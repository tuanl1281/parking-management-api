pipeline {
  agent any

  options {
    ansiColor('xterm')
  }

  environment {
    /*  ***** DEFAULT *****
        PROJECT:                            hrs
        PROJECT:                            api

        CLIENT                              develop
        CLIENT_CREDENTIAL                   develop_hrs
        CLIENT_RUNTIME_PATH_SUFFIX          /home/none
        CLIENT_RUNTIME_PATH                 /home/none/develop_hrs_api

        DOCKER_IMAGE                        titkul-dev/hrs-api

        DOCKER_REGISTRY_URL                 registry.gitlab.com
        DOCKER_REGISTRY_CREDENTIAL          jenkins_titkul_gitlab

        DOCKER_COMPOSE_APP                  hrs_api
        *******************
    */

    PROJECT                                  = "user-management"
    PROJECT_MODULE                           = "api"

    CLIENT                                   = "${GIT_BRANCH.tokenize('/').pop()}"
    CLIENT_CREDENTIAL                        = "${CLIENT}_${PROJECT}"
    CLIENT_RUNTIME_PATH_SUFFIX               = "/home/none"
    CLIENT_RUNTIME_PATH                      = "${CLIENT_RUNTIME_PATH_SUFFIX}/${CLIENT}_${PROJECT}_${PROJECT_MODULE}"

    DOCKER_IMAGE                             = "devops/${PROJECT}-${PROJECT_MODULE}"

    DOCKER_REGISTRY_URL                      = "ghcr.io"
    DOCKER_REGISTRY_CREDENTIAL               = "j4lrs.github"

    DOCKER_COMPOSE_APP                       = "${PROJECT}_${PROJECT_MODULE}"
  }

  stages {
    stage("Build") {
      agent {
        node {
          label "master"
        }
      }

      when {
        anyOf {
          expression { BRANCH_NAME == "develop" }
          expression { BRANCH_NAME == "master" }
        }
      }

      steps {
        /* Config */
        script {
          env.DOCKER_TAG = "latest"
          if (BRANCH_NAME == "develop") {
            env.DOCKER_TAG = "develop"
          }
        }
        /* Build */
        sh '''
          docker build -t $DOCKER_REGISTRY_URL/$DOCKER_IMAGE:''' + env.DOCKER_TAG + ''' .
        '''
        /* Push */
        withCredentials([[
          $class: 'UsernamePasswordMultiBinding',
          credentialsId: DOCKER_REGISTRY_CREDENTIAL,
          usernameVariable: 'DOCKER_REGISTRY_USERNAME',
          passwordVariable: 'DOCKER_REGISTRY_PASSWORD'
        ]]) {
          sh '''
            echo $DOCKER_REGISTRY_PASSWORD | docker login $DOCKER_REGISTRY_URL --username $DOCKER_REGISTRY_USERNAME --password-stdin
            docker push $DOCKER_REGISTRY_URL/$DOCKER_IMAGE:''' + env.DOCKER_TAG + '''
          '''
        }
        /* Clean */
        sh '''
          docker image rm $DOCKER_REGISTRY_URL/$DOCKER_IMAGE:''' + env.DOCKER_TAG + '''
        '''
      }
    }

    stage("Deploy") {
      agent {
        node {
          label "master"
        }
      }

      when {
        anyOf {
          expression { BRANCH_NAME == "develop" }
          expression { BRANCH_NAME ==~ /deploy\/(.*)/ }
        }
      }

      steps {
        /* Execute */
        withCredentials([
          sshUserPrivateKey(
            credentialsId: "$CLIENT_CREDENTIAL",
            usernameVariable: 'CLIENT_USERNAME',
            keyFileVariable: 'CLIENT_KEY',
          )
        ]) {
          /* Config */
          script {
            env.DOCKER_TAG = "latest"
            if (BRANCH_NAME == "develop") {
              env.DOCKER_TAG = "develop"
            }
          }
          /* Create runtime folder */
          sh '''
            ssh -i $CLIENT_KEY -oStrictHostKeyChecking=no -oUserKnownHostsFile=/dev/null $CLIENT_USERNAME "
              mkdir -p $CLIENT_RUNTIME_PATH;
            "
          '''
          /* Update file accessories */
          sh "scp -i $CLIENT_KEY -oStrictHostKeyChecking=no -oUserKnownHostsFile=/dev/null .env $CLIENT_USERNAME:$CLIENT_RUNTIME_PATH/.env"
          sh "scp -i $CLIENT_KEY -oStrictHostKeyChecking=no -oUserKnownHostsFile=/dev/null appsettings.json $CLIENT_USERNAME:$CLIENT_RUNTIME_PATH/appsettings.json"
          sh "scp -i $CLIENT_KEY -oStrictHostKeyChecking=no -oUserKnownHostsFile=/dev/null docker-compose.yml $CLIENT_USERNAME:$CLIENT_RUNTIME_PATH/docker-compose.yml"
          /* Restart service */
          withCredentials([[
            $class: 'UsernamePasswordMultiBinding',
            credentialsId: DOCKER_REGISTRY_CREDENTIAL,
            usernameVariable: 'DOCKER_REGISTRY_USERNAME',
            passwordVariable: 'DOCKER_REGISTRY_PASSWORD'
          ]]) {
            sh '''
              ssh -i $CLIENT_KEY -oStrictHostKeyChecking=no -oUserKnownHostsFile=/dev/null $CLIENT_USERNAME "
                cd $CLIENT_RUNTIME_PATH;
                source .env;
                export TAG=''' + env.DOCKER_TAG + ''';
                export COMPOSE_PROJECT_NAME=''' + CLIENT + '''_''' + PROJECT + '''_''' + PROJECT_MODULE + ''';
                echo $DOCKER_REGISTRY_PASSWORD | docker login $DOCKER_REGISTRY_URL --username $DOCKER_REGISTRY_USERNAME --password-stdin;
                docker-compose stop;
                docker-compose rm -f;
                docker-compose pull $DOCKER_COMPOSE_APP;
                docker-compose up -d;
              "
            '''
          }
        }
      }
    }
  }
}