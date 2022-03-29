# Addons.AwsSdk

This package will contain tools that are missing from the Official AWSSDK:

## CloudFormationClient

The official CloudFormationClient only allows to return pages of export names where you need to repeat the calls until you find the name you want.
This client will let you send an export name and will return the matched value or an exception if not found.

usage:

    var client = new CloudFormationClient(_accessKey, _secretKey, _regionName);
    var response = await client.GetExportValueByExportNameAsync(_serviceExportName);

## Nuget

https://www.nuget.org/packages/Addons.AwsSdk
