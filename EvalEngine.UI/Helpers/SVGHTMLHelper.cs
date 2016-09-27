// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JQueryUIDatePickerHelper.cs" company="RTI, Inc.">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The jquery UI datepicker helper. Helps format date data, used for split date select boxes. See: http://www.rajeeshcv.com/post/details/31/jqueryui-datepicker-in-asp-net-mvc
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.UI.Helpers
{
    using System.Threading;
    using System.Web.Mvc;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web.Helpers;
    using EvalEngine.UI.Models;
    using System.Text;
    using Svg;

    /// <summary>
    /// SVGHTMLhelper class.
    /// </summary>
    public static class SVGHTMLHelper
    {
        public static byte[] ToByteArray(Bitmap image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static ActionResult SVGify(ReportChart chart)
        {
            var byteArray = Encoding.ASCII.GetBytes(chart.Chart);

            using (var stream = new MemoryStream(byteArray))
            {
                var svgDocument = SvgDocument.Open(stream);
                var bitmap = svgDocument.Draw();
                byte[] arr = ToByteArray(bitmap, ImageFormat.Bmp);
                WebImage image = new WebImage(arr);
                var imgfile = image.GetBytes("image/jpeg");
                //return File(imgfile, "image/jpeg");
                return null;
            }
        }
    }
}
