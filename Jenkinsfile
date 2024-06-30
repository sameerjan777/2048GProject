pipeline {
    agent any

    environment {
        UNITY_VERSION = '2022.3.28f1' // Specify your Unity version
        UNITY_PATH = "/Applications/Unity/Hub/Editor/${UNITY_VERSION}/Unity.app/Contents/MacOS/Unity"
        PROJECT_PATH = "${WORKSPACE}" // Jenkins workspace path
        BUILD_PATH = "${WORKSPACE}/Builds/Android"
        LOG_FILE = "${WORKSPACE}/unity_build_log.txt"
        APK_NAME = "2048Practice.apk"
    }

    options {
        timeout(time: 30, unit: 'MINUTES') // Adjust timeout as necessary
    }

    stages {
        stage('Checkout Code') {
            steps {
                cleanWs()
                checkout scm
            }
        }

        stage('Build Unity Project (Android)') {
            steps {
                script {
                    // Create build directory if it doesn't exist
                    sh "mkdir -p ${BUILD_PATH}"

                    // Build the Android APK
                    def result = sh(
                        script: """
                            ${UNITY_PATH} -batchmode -quit -projectPath ${PROJECT_PATH} -executeMethod BuildScript.BuildAndroid -buildTarget Android -logFile ${LOG_FILE}
                        """,
                        returnStatus: true
                    )

                    if (result != 0) {
                        error "Unity build failed with exit code: ${result}"
                    }

                    // Print contents of the build path after build
                    sh "ls -la ${BUILD_PATH}"
                }
            }
        }

        stage('Verify Build Output') {
            steps {
                sh "ls -la ${BUILD_PATH}"
            }
        }

        stage('Archive Build') {
            steps {
                archiveArtifacts artifacts: "${BUILD_PATH}/${APK_NAME}", allowEmptyArchive: false
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
