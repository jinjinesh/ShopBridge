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
				bat "docker build -t jinjinesh/i_jineshjain_${env.branch_name}:${BUILD_NUMBER} --no-cache -f ShopBridge/DockerFile ."
			}
		}
		stage('Containers') {
			parallel {
				stage ('PushtoDTR') {
					steps {
						echo "docker push i_jineshjain_${env.branch_name}:${BUILD_NUMBER}"
					}
				}
				stage ('PreContainerCheck') {
					steps {
						powershell label: '', script: '''$cID = $(docker ps -qf "name=c_jineshjain_${env.branch_name}")
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
				echo "${env.branch_name}"
				powershell label: '', script: '''
				$branch = ${env.branch_name};
				echo $branch;
				if($branch -eq "master"){
					echo "docker run -d -p 6000:80 --name c_jineshjain_${env.branch_name} jinjinesh/i_jineshjain_${env.branch_name}:${BUILD_NUMBER}"
					docker run -d -p 6000:80 --name c_jineshjain_${env.branch_name} jinjinesh/i_jineshjain_${env.branch_name}:${BUILD_NUMBER}
				} else {
					echo  "docker run -d -p 6100:80 --name c_jineshjain_${env.branch_name} jinjinesh/i_jineshjain_${env.branch_name}:${BUILD_NUMBER}"
					docker run -d -p 6100:80 --name c_jineshjain_${env.branch_name} jinjinesh/i_jineshjain_${env.branch_name}:${BUILD_NUMBER}
				}				
				'''
			}
		}
		
	}
}