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
		stage ('Docker deployment') {
			steps {
				powershell label: '', script: '''$port = 6000;
				$branch = ${env.branch_name};
				echo $branch;
				if($branch -ne "master"){
					$port = 6100
				}
				echo $port;
				docker run -d -p $port:80 --name shopbridge jinjinesh/shopbridge:14
				'''
			}
		}
		
	}
}