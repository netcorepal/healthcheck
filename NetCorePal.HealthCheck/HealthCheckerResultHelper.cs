using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("NetCorePal.HealthCheck.Web")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetCorePal.HealthCheck.AspNetCore")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetCorePal.HealthCheck.Test")]
namespace NetCorePal.HealthCheck
{
    static class HealthCheckerResultHelper
    {
        public static string ToHtml(this HealthCheckResult[] results)
        {
            var tempHtml = @"
<!DOCTYPE html>
<html>
<head>
	<title>健康检查结果</title>
	<style type='text/css'>
		*{{padding: 0;margin: 0;}}
		.listTableBox{{width: 100%;}}
		.listTableBox th{{height:32px;}}
		.listTableBox td {{height: 40px; color: #666;}}
		.listTableBox .ListDate td {{  text-align: center; border-bottom: #ddd 1px solid; border-right: #ddd 1px solid; padding-left: 0px;}}
		.listTableBox th{{ text-align: center; border-bottom: #ddd 1px solid;padding-left: 0px; background: #32acda; color:#fff;}}
		.newListDateTable .the-sum-box{{ min-height: 20px;}}
		.ListDate_current{{background: #f0f0f0;}}
		.ListDate td{{border-bottom:#ddd 1px solid;text-align: left;padding-left: 12px;}}
		.ListDate td{{height:35px; color:#666;}}
		.ListDate:hover td{{background:#e4f1f7; cursor:pointer;}}
		.ListDate:hover td{{cursor:pointer; }}
        .ErrorColor td{{color:red;}}
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
	            {0}
            </tbody>
        </table>
	</div>
    <script type='text/javascript'>
        var showMsg = function(obj){{
                if(obj.parentNode.nextSibling.style.display=='none'){{
                   obj.parentNode.nextSibling.removeAttribute('style','display:none');
                }}
                else{{
                    obj.parentNode.nextSibling.setAttribute('style','display:none');
                }}
            }}
    </script>
</body>
</html>";
            var tempTr = @"";
            foreach (var item in results)
            {
                if (item.IsHealthy)
                {
                    tempTr += $@"<tr class='ListDate ListDate_current'>
	                <td>{item.Name}</td>
	                <td>{item.IsHealthy}</td>
	                <td>{item.Elapsed}</td>
	                <td>{item.Message}</td>
	            </tr>";
                }
                else
                {
                    tempTr += $@"<tr class='ErrorColor ListDate ListDate_current'>
	                <td>{item.Name}</td>
	                <td>{item.IsHealthy}</td>
	                <td>{item.Elapsed}</td>
	                <td onclick='showMsg(this)'>{ GetShortString(item.Message, 20)}...</td>
                </tr><tr class='ErrorColor ListDate ListDate_current' style='display:none;'><td colspan='5'>{item.Message}</td></tr>
                ";
                }

            }
            return string.Format(tempHtml, tempTr);
        }

        static string GetShortString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
    }
}
