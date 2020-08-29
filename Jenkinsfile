pipeline {
	agent any
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
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
				checkout scm
			}
		}
		stage('Nuget restore') {
			steps {
				bat "dotnet restore"
			}
		}
		stage('Start sonarqube analysis') {
			when {
				branch 'master'
			}
			steps {
				withSonarQubeEnv('Test_Sonar') {
					echo "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:shopbridge /n:shopbridge /v:1.0"
					bat "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:shopbridge /n:shopbridge /v:1.0"
				}
			}
		}
		stage('Build') {
			steps {
				bat "dotnet build -c Release -o app/build"
			}
		}
		stage('Stop sonarqube analysis') {
			when {
				branch 'master'
			}
			steps {
				withSonarQubeEnv('Test_Sonar') {
					bat "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
				}
			}
		}
		stage('Release artificats') {
			steps {
				bat "dotnet publish -c Release -o app/publish"
			}
		}
		stage('Docker image') {
			steps {
				bat "docker build -t jinjinesh/shopbridge:${BUILD_NUMBER} --no-cache -f ShopBridge/DockerFile ."
			}
		}
		stage('Containers') {
			parallel {
				stage ('PushtoDTR') {
					steps {
						bat "docker push jinjinesh/shopbridge:${BUILD_NUMBER}"
					}
				}
				stage ('PreContainerCheck') {
					steps {
						powershell label: '', script: '''$cID = $(docker ps -qf "name=shopbridge")
						if($cID){
							docker container stop $cID;
							docker rm $cID;
						}'''
					}
				}
			}
		}
		stage ('Docker deployment') {
			steps {
				powershell label: '', script: '''$port = 6000
				$branch = ${env.branch_name};
				if($branch -ne "master"){
					$port = 6100
				}
				docker run -d -p $port:80 --name shopbridge jinjinesh/shopbridge:${BUILD_NUMBER}
				'''
			}
		}
		
	}
}