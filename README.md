# Contoso.Online.Orders
The .NET 5 version of Contoso Online Orders API. 

# Deploying order api to Kubernetes

- All the K8s related scripts are kept under **deploy/k8s** directory.
- It has both types of `yaml`. Each yaml include `deployment` and respective loadbalancer service as well.

### Deploy with Azure Cosmos as persistent data store

- Go to the directory `Contoso.Online.Orders`
- Login to the dockerhub (docker.io) and keep your **YOUR-DOCKERHUB-ID** handy you need that in the next step. 
- Run below command :

    ```text
    docker build -t <YOUR-DOCKERHUB-ID>/contosoonlineordersapi-cosmos -f ContosoOnlineOrders.Api/Dockerfile .
    ```

- Build steps should look like as per below :

    ![](media/docker-image-build.png)

- Once it gets built successfully push that to the dockerhub repository. You could do that by using the following command :

    ```text
    docker push <YOUR-DOCKERHUB-ID>/contosoonlineordersapi-cosmos
    ```

    ![](media/docker-push.png)
    
- You need to make changes in the respective k8s yaml file as well to point to the right docker repository. In this case file name is `apideploy-cosmos.yaml`

- You will also need to update the `ConnectionStrings__ContosoOrdersConnectionString` environment variable to point it to the right Azure Cosmos DB instance.
- Then you go to the directory **Contoso.Online.Orders/deploy/k8s**
- And run below command :

    ```text
    kubectl apply -f .\apideploy-cosmos.yaml
    ```

### Deploy with in-memory cache

- To deploy the in-memory version of the API instance, you need to make sure following code snippet are enabled and you also need to make sure `AddCosmosDbStorage` step is disabled. For e.g :

    ```csharp
    services.AddMemoryCache();
    services.AddSingleton<IStoreDataService, MemoryCachedStoreServices>();
    
    //services.AddCosmosDbStorage(Configuration.GetConnectionString("ContosoOrdersConnectionString"));    
    ```

- And then like above you need to generate a new docker image and publish to the dockerhub to use it for k8s specific deployment.
- You can use below commands for that :

    ```text
    docker build -t <YOUR-DOCKERHUB-ID>/contosoonlineordersapi -f ContosoOnlineOrders.Api/Dockerfile .
    
    docker push <YOUR-DOCKERHUB-ID>/contosoonlineordersapi    
    ```
- In this scenario you don't need to pass on any connection string as it's going to use in-memory data store. So you can directly run the following command :

    ```text
    kubectl apply -f .\apideploy-inmem.yaml
    ```

Once you make successful deployment of both the services you should be able to see something as per below :

   ![](media/kubectl-services.png)

- Now you can use respective services using `http://<EXTERNAL-IP>/>`