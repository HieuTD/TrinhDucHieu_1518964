namespace WebApi.Helper.Common
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
  <head>
  </head>
  <body style=""margin:0;font-family: Arial, Helvetica, sans-serif;"">
    <div style=""width:400px;height: auto;background: #eee8dc no-repeat;"">
      <div style=""width:400px; height:auto;padding:15px; background:#fff;position:absolute;top:20%;left:50%;transform:translate(-50%,-20%"">
        <div>
          <h1>Đặt lại mật khẩu của bạn</h1>
          <hr>
          <p style=""color:black"">Bạn nhận được email này bời bạn đã yêu cầu thiết lập lại mật khẩu của tài khoản tại hệ thống DH Store.</p>

          <p style=""color:black"">Nhấn vào nút dưới đây để chuyển hướng sang trang thiết lập lại mật khẩu.</p>

          <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efd;padding:10px;border:none;
          color:white;border-radius:4px;display:block;margin:0 auto;width:50%;text-align:center;text-decoration:none"">Reset</a><br>

          <p>Best Regards,<br><br>
          DH Store</p>
        </div>
      </div>
    </div>
  </body>
</html>";
        }
    }
}
