pipeline {
    agent any
    
    environment {
        UNITY_PATH = "/Applications/Unity/Hub/Editor/2022.3.28f1/Unity.app/Contents/MacOS/Unity"
        PROJECT_PATH = "/Users/rodeorack/.jenkins/workspace/${env.JOB_NAME}"
        GIT_REPO = "https://github.com/sameerjan777/2048GProject.git"
        GIT_BRANCH = "main"
        EMAIL_RECIPIENT = "sameer.ar.jan@gmail.com"
    }
    
    stages {
        stage('Checkout Code') {
            steps {
                cleanWs()
                checkout scmGit(branches: [[name: '*/main']], extensions: [], userRemoteConfigs: [[credentialsId: 'githubCredentials1', url: "${GIT_REPO}"]])
            }
        }
        
        stage('Run Unit Tests on Android') {
            steps {
                script {
                    def resultFile = "${PROJECT_PATH}/TestResults_Android.txt"
                    def logFile = "${PROJECT_PATH}/TestLog_Android.log"
                    
                    // Run Unity tests
                    def result = sh(script: "${UNITY_PATH} -projectPath ${PROJECT_PATH} -runTests -testPlatform playmode -buildTarget Android -testResults ${resultFile} -logFile ${logFile} -batchmode -nographics -quit", returnStatus: true)
                    
                    // Fail the build if the result is non-zero
                    if (result != 0) {
                        currentBuild.result = 'FAILURE'
                        error('Unit tests failed on Android!')
                    }
                    
                    // Parse the test results for failures
                    def testResults = readFile(resultFile)
                    if (testResults.contains('<failure>') || testResults.contains('<error>')) {
                        currentBuild.result = 'FAILURE'
                        error('Unit tests failed on Android (XML check)!')
                    }
                }
            }
        }
        
        stage('Run Unit Tests on iOS') {
            steps {
                script {
                    def resultFile = "${PROJECT_PATH}/TestResults_iOS.xml"
                    def logFile = "${PROJECT_PATH}/TestLog_iOS.log"
                    
                    // Run Unity tests
                    def result = sh(script: "${UNITY_PATH} -projectPath ${PROJECT_PATH} -runTests -testPlatform playmode -buildTarget iOS -testResults ${resultFile} -logFile ${logFile} -batchmode -nographics -quit", returnStatus: true)
                    
                    // Fail the build if the result is non-zero
                    if (result != 0) {
                        currentBuild.result = 'FAILURE'
                        error('Unit tests failed on iOS!')
                    }
                    
                    // Parse the test results for failures
                    def testResults = readFile(resultFile)
                    if (testResults.contains('<failure>') || testResults.contains('<error>')) {
                        currentBuild.result = 'FAILURE'
                        error('Unit tests failed on iOS (XML check)!')
                    }
                }
            }
        }
    }
    
    post {
        failure {
            mail to: "${EMAIL_RECIPIENT}",
                 subject: "Jenkins Build Failed: ${env.JOB_NAME} - ${env.BUILD_NUMBER}",
                 body: "The Jenkins build failed at stage: ${env.STAGE_NAME}. Please check the Jenkins console output for more details."
        }
        success {
            mail to: "${EMAIL_RECIPIENT}",
                 subject: "Jenkins Build Succeeded: ${env.JOB_NAME} - ${env.BUILD_NUMBER}",
                 body: "The Jenkins build succeeded at stage: ${env.STAGE_NAME}. Please check the Jenkins console output for more details."
        }
    }
}
