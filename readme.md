# NetCorePal.HealthCheck

Health check tool for .net and .net core

## Install

To install SchoolPal.Toolkit.Pinyins from the Package Manager Console, run the following command:
```
Install-Package NetCorePal.HealthCheck
```

For asp.net or asp.net mvc 
```
Install-Package NetCorePal.HealthCheck.Web
```

For asp.net core
```
Install-Package NetCorePal.HealthCheck.AspNetCore
```


## How to use

```
HealthCheckerManager.Manager.Add(new YourChecker());
```