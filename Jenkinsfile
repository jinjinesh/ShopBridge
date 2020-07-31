pipeline {
	agent any
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
		stage('Build') {
			steps {
				bat "dotnet build -c Release -o Shopbridge/app/build"
			}
		}
		stage('Test') {
			steps {
				bat "dotnet test --test-adapter-path:. --logger:\"nunit;LogFilePath=TestResults/test-result.xml\" /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=opencover /p:ExcludeByFile=\"**/Migrations/**/*.cs\""
			}
		}
		stage('Release artificats') {
			steps {
				bat "dotnet publish -c Release -o Shopbridge/app/publish"
			}
		}
	}
}