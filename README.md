# Addons.AwsSdk

This package will contain tools that are missing from the Official AWSSDK:

## CloudFormationClient

The official CloudFormationClient only allows to return pages of export names where you need to repeat the calls until you find the name you want.
This client will let you send an export name and will return the matched value or an exception if not found.

usage:

    var client = new CloudFormationClient(_accessKey, _secretKey, _regionName);
    var response = await client.GetExportValueByExportNameAsync(_serviceExportName);
 
 
## ApiGatewayClient

The official sdk doesn't have a signing option for calling the api gateway from external code or an option to send the request.
This client will let you send a json and it will sign and send it to the api gateway. Signing happens with an algoritm Implemented in another Nuget.

usage:

    var client = new ApiGatewayClient(_accessKey, _secretKey, _regionName);
    var response = await client.PostAsync(_address, HttpMethod.Post, _json);

## Nuget

https://www.nuget.org/packages/Addons.AwsSdk/2.1.1
