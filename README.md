# MPP File to Json Lambda App

This app downloads given mpp url file and returns as json.

- Build with docker and push to Aws ECR. First docker login if you haven't

      aws ecr get-login-password --region eu-central-1 | docker login --username AWS --password-stdin {account-id}.dkr.ecr.eu-central-1.amazonaws.com

      docker build . -t {account-id}.dkr.ecr.eu-central-1.amazonaws.com/{container-name}

      docker push {account-id}.dkr.ecr.eu-central-1.amazonaws.com/{container-name}:latest

- Create your lambda function from image on Aws console
