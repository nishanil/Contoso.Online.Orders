# Deploy dev version of contoso order online api
kubectl apply -f .\dev\apideploy-dev.yaml

# Deploy prod version of contoso order online api
kubectl apply -f .\prod\apideploy-prod.yaml

kubectl apply -f .\ingress.yaml