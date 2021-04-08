using ApiServer.Extensions.AOP;
using ApiServer.Model.Model.ViewModel;
using Autofac.Extras.DynamicProxy;
using FluentValidation;

namespace ApiServer.Extensions.Validation
{
	public class UserValidator : AbstractValidator<SysUser>
	{
		public UserValidator()
		{
			CascadeMode = CascadeMode.Stop;
			RuleFor(x => x.username).NotEmpty().WithMessage("用户名不能为空！");
			RuleFor(x => x.password).NotEmpty().WithMessage("密码不能为空！");
		}
	}
}
