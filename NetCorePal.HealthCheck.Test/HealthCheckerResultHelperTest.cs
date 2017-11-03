using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using NetCorePal.HealthCheck;
namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class HealthCheckerResultHelperTest
    {

        [TestMethod]
        public void HealthCheckerResultHelper_ToHtml_Test()
        {
            var results = new HealthCheckResult[] { };
            var html = results.ToHtml();
            Assert.IsNotNull(html);
            Assert.AreEqual(emptyResultHtml, html);


            results = new HealthCheckResult[] {
                new HealthCheckResult{ Name="c1", Elapsed=10, IsHealthy=true, Message="a1" },
                new HealthCheckResult{ Name="c2",Elapsed=12 , IsHealthy=false, Message="error"}
            };
            html = results.ToHtml();
            Assert.IsNotNull(html);
            Assert.AreEqual(resultHtml, html);

        }

        #region html
        const string emptyResultHtml = @"<!DOCTYPE html>
<html>
<head>
	<title>健康检查结果</title>
	<style type='text/css'>
		*{padding: 0;margin: 0;}
		.listTableBox{width: 100%;}
		.listTableBox th{height:32px;}
		.listTableBox td {height: 40px; color: #666;}
		.listTableBox .ListDate td {  text-align: center; border-bottom: #ddd 1px solid; border-right: #ddd 1px solid; padding-left: 0px;}
		.listTableBox th{ text-align: center; border-bottom: #ddd 1px solid;padding-left: 0px; background: #32acda; color:#fff;}
		.newListDateTable .the-sum-box{ min-height: 20px;}
		.ListDate_current{background: #f0f0f0;}
		.ListDate td{border-bottom:#ddd 1px solid;text-align: left;padding-left: 12px;}
		.ListDate td{height:35px; color:#666;}
		.ListDate:hover td{background:#e4f1f7; cursor:pointer;}
		.ListDate:hover td{cursor:pointer; }
        .ErrorColor td{color:red;}
	</style>
</head>
<body>
	<div>
		<table border='0' cellpadding='0' cellspacing='0' class='listTableBox'>
            <tbody>
	            <tr>
	                <th width='20%'><span>名称</span></th>
	                <th width='15%'><span>是否健康</span></th>
	                <th width='15%'><span>响应时间(毫秒)</span></th>
	                <th width='25%'><span>信息</span></th>
	            </tr>
	            
            </tbody>
        </table>
	</div>
    <script type='text/javascript'>
        var showMsg = function(obj){
                if(obj.parentNode.nextSibling.style.display=='none'){
                   obj.parentNode.nextSibling.removeAttribute('style','display:none');
                }
                else{
                    obj.parentNode.nextSibling.setAttribute('style','display:none');
                }
            }
    </script>
</body>
</html>";
        const string resultHtml = @"<!DOCTYPE html>
<html>
<head>
	<title>健康检查结果</title>
	<style type='text/css'>
		*{padding: 0;margin: 0;}
		.listTableBox{width: 100%;}
		.listTableBox th{height:32px;}
		.listTableBox td {height: 40px; color: #666;}
		.listTableBox .ListDate td {  text-align: center; border-bottom: #ddd 1px solid; border-right: #ddd 1px solid; padding-left: 0px;}
		.listTableBox th{ text-align: center; border-bottom: #ddd 1px solid;padding-left: 0px; background: #32acda; color:#fff;}
		.newListDateTable .the-sum-box{ min-height: 20px;}
		.ListDate_current{background: #f0f0f0;}
		.ListDate td{border-bottom:#ddd 1px solid;text-align: left;padding-left: 12px;}
		.ListDate td{height:35px; color:#666;}
		.ListDate:hover td{background:#e4f1f7; cursor:pointer;}
		.ListDate:hover td{cursor:pointer; }
        .ErrorColor td{color:red;}
	</style>
</head>
<body>
	<div>
		<table border='0' cellpadding='0' cellspacing='0' class='listTableBox'>
            <tbody>
	            <tr>
	                <th width='20%'><span>名称</span></th>
	                <th width='15%'><span>是否健康</span></th>
	                <th width='15%'><span>响应时间(毫秒)</span></th>
	                <th width='25%'><span>信息</span></th>
	            </tr>
	            <tr class='ListDate ListDate_current'>
	                <td>c1</td>
	                <td>True</td>
	                <td>10</td>
	                <td>a1</td>
	            </tr><tr class='ErrorColor ListDate ListDate_current'>
	                <td>c2</td>
	                <td>False</td>
	                <td>12</td>
	                <td onclick='showMsg(this)'>error...</td>
                </tr><tr class='ErrorColor ListDate ListDate_current' style='display:none;'><td colspan='5'>error</td></tr>
                
            </tbody>
        </table>
	</div>
    <script type='text/javascript'>
        var showMsg = function(obj){
                if(obj.parentNode.nextSibling.style.display=='none'){
                   obj.parentNode.nextSibling.removeAttribute('style','display:none');
                }
                else{
                    obj.parentNode.nextSibling.setAttribute('style','display:none');
                }
            }
    </script>
</body>
</html>";
        #endregion
    }
}
