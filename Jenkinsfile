pipeline {
	agent any
	environment {
		scannerHome = tool name: 'SonarMsBuild', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
	}
	options {
	  buildDiscarder logRotator(daysToKeepStr: '10', numToKeepStr: '5')
	  disableConcurrentBuilds()
	  timeout(time: 1, unit: 'HOURS')
	  timestamps()
	}
	stages {
		stage('Checkout') {
			steps {
				echo "=======Checkout started==========="
				checkout scm
				echo "=======Checkout completed========="
			}
		}
		stage('Nuget') {
			steps {
				bat "dotnet restore"
			}
		}
		stage('Begin sonarqube analysis') {
			steps {
				echo "job name ${JOB_NAME}"
				withSonarQubeEnv('sonarqube') {
					bat "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\".NetCore\" /n:\".NetCore\" /v:3.0 /d:sonar.cs.opencover.reportsPaths=ShopBridge.Test/TestResults/coverage.opencover.xml /d:sonar.coverage.exclusions=\"**Test*.cs\""
				}
			}
		}
		stage('Build') {
			steps {
				bat "dotnet build -c Release -o app/build"
			}
		}
		stage('Test') {
			steps {
				bat "dotnet test --test-adapter-path:. --logger:\"nunit;LogFilePath=TestResults/test-result.xml\" /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=opencover /p:ExcludeByFile=\"**/Migrations/**/*.cs\""
			}
		}
		stage('End sonarqube analysis') {
			steps {
				withSonarQubeEnv('sonarqube') {
					bat "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
				}
			}
		}
		stage('Release artificats') {
			steps {
				bat "dotnet publish -c Release -o app/publish"
			}
		}
		stage('Publish open cover report') {
			steps {
				publishCoverage adapters: [opencoverAdapter(mergeToOneReport: true, path: 'ShopBridge.Test/TestResults/coverage.opencover.xml')], sourceFileResolver: sourceFiles('NEVER_STORE')
			}
		}
		stage('Create docker image') {
			steps {
				bat "docker build -t jinjinesh/shopbridge:${BUILD_NUMBER} --no-cache -f ShopBridge/DockerFile ."
			}
		}
		stage ('push') {
			steps {
				bat "docker push jinjinesh/shopbridge:${BUILD_NUMBER}"
			}
		}
		stage ('Stop running container') {
			steps {
				echo "=======stop container==========="
			}
		}
		stage ('run docker') {
			steps {
				bat "docker run -d -p 9090:80 --name shopbridge jinjinesh/shopbridge:${BUILD_NUMBER}"
			}
		}
		
	}
	post {
		always {
			nunit testResultsPattern: 'ShopBridge.Test/TestResults/test-result.xml'
		}
	}
}