# NetCorePal.HealthCheck

专为.net和.net core开发的健康检查工具，仅需极少的几行代码就可以完成服务以及服务所依赖的项目的健康检查，并且支持图形化方式呈现

## 如何安装

从nuget库安装基础组件NetCorePal.HealthCheck
```
Install-Package NetCorePal.HealthCheck
```

如果使用asp.net或者asp.net mvc
```
Install-Package NetCorePal.HealthCheck.Web
```
 
如果使用asp.net webapi
```
Install-Package NetCorePal.HealthCheck.WebApi
```

如果使用asp.net owin web
```
Install-Package NetCorePal.HealthCheck.Owin
```


如果使用.net core
```
Install-Package NetCorePal.HealthCheck.AspNetCore
```


## 如何使用


#### 添加健康检查
```
using NetCorePal.HealthCheck；

HealthCheckerManager.Manager.Add(new YourChecker()); //你对健康检查的业务逻辑处理
或者
HealthCheckerManager.Manager.Add("mycheckerName",()=>{ 
//你自己的业务逻辑
    return new HealthCheckResult(){}; //add func as a checker

}); 

HealthCheckerManager.Manager.AddAllDbConnectionHealthCheckers(); // 自动将web.config （.NET Framework 4.5+）中的数据库连接字符串作为依赖项加入健康检查 ，连接字符串中如果没有加入ProviderName，则默认使用MySQL

HealthCheckerManager.Manager.AddHttpHeadHealthChecker("checkername", "url");  // 有些情况下，你的项目只需返回Head信息来表示健康状况（比如阿里云的SLB对健康检查的支持），使用这个方法将不会对你项目的其他依赖项进行检查 
```

#### 如何获取健康检查结果

```
var results = HealthCheckerManager.Manager.CheckAllAsync().Result;
或者
var results = await HealthCheckerManager.Manager.CheckAllAsync(); //异步方法
```

#### 使用浏览器查看健康检查结果

为了防止健康检查被外部攻击，我们支持使用密钥方式访问健康检查，一旦密钥不匹配，我们会立即返回失败结果而不会执行检查逻辑

asp.net mvc
```
Global.asax.cs

using NetCorePal.HealthCheck;

protected void Application_Start()
{
    //  其他代码...
    RouteTable.Routes.UseHealthCheck(url: "healthcheck", apiKey: "密钥");
}
```

asp.net web api
```
Global.asax.cs

using NetCorePal.HealthCheck;

protected void Application_Start()
{
    //  其他代码...
    GlobalConfiguration.Configuration.UseHealthCheck(url: "/healthcheck", apiKey: "密钥");
}
```

asp.net owin web
```
Startup.cs

using NetCorePal.HealthCheck;

public void Configuration(IAppBuilder app)
{
    //  其他代码...
     app.UseHealthCheck(url: "/healthcheck", apiKey: "密钥");
}
```


asp.net core
```
Startup.cs

using NetCorePal.HealthCheck;

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseHealthCheck(url: "/healthcheck", apiKey: "密钥");
    
    //  其他代码...
}
```

浏览器中输入网址：
```
http://你的基础路径/healthcheck?apikey=密钥
```

curl：
```
curl http://你的基础路径/healthcheck?apikey=密钥
```

head方法检查（head请求不需要密钥）
```
curl --head http://你的基础路径/healthcheck
```
