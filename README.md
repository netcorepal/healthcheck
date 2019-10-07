# NetCorePal.HealthCheck

Health check tool for .net and .net core

## Install

To install NetCorePal.HealthCheck from the Package Manager Console, run the following command:
```
Install-Package NetCorePal.HealthCheck
```

For asp.net or asp.net mvc 
```
Install-Package NetCorePal.HealthCheck.Web
```

For asp.net or asp.net web api 
```
Install-Package NetCorePal.HealthCheck.WebApi
```

For asp.net or asp.net owin web
```
Install-Package NetCorePal.HealthCheck.Owin
```


For asp.net core
```
Install-Package NetCorePal.HealthCheck.AspNetCore
```


## How to use


#### How to add checker
```
using NetCorePal.HealthCheck；

HealthCheckerManager.Manager.Add(new YourChecker()); //add your own checker

HealthCheckerManager.Manager.AddAllDbConnectionHealthCheckers(); //add all connectionstring in web.config （.NET Framework 4.5+）

HealthCheckerManager.Manager.Add("mycheckerName",()=>{ 
//you own check code
    return new HealthCheckResult(){}; //add func as a checker

}); 

HealthCheckerManager.Manager.AddHttpHeadHealthChecker("checkername", "url");
```

#### How to get check result

```
var results = HealthCheckerManager.Manager.CheckAllAsync().Result;
or
var results = await HealthCheckerManager.Manager.CheckAllAsync(); //in async func
```

#### View check results in website

For asp.net or asp.net mvc
```
Global.asax.cs

using NetCorePal.HealthCheck;

protected void Application_Start()
{
    //Your own code....
    RouteTable.Routes.UseHealthCheck(url: "healthcheck", apiKey: "yourapikey");
}
```

For asp.net or asp.net web api
```
Global.asax.cs

using NetCorePal.HealthCheck;

protected void Application_Start()
{
    //Your own code....
    GlobalConfiguration.Configuration.UseHealthCheck(url: "healthcheck", apiKey: "yourapikey");
}
```

For asp.net or asp.net owin web
```
Startup.cs

using NetCorePal.HealthCheck;

public void Configuration(IAppBuilder app)
{
    //Your own code....
     app.UseHealthCheck(url: "healthcheck", apiKey: "yourapikey");
}
```


For asp.net core
```
Startup.cs

using NetCorePal.HealthCheck;

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHealthCheck(url: "/healthcheck", apiKey: "yourapikey");
    //your own code...
}
```