using FluentValidation;

namespace Organizations.Model.Validators
{
    public static class RuleBuilderEx
    {
        public static IRuleBuilderOptions<T, string> WebsiteLink<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Matches(@"^((?:http|https):\/\/)?(?:[\w\.\-\+]+:{0,1}[\w\.\-\+]*@)?(?:[a-z0-9\-\.]+)
(?::[0-9]+)?(?:\/|\/(?:[\w#!:\.\?\+=&%@!\-\/\(\)]+)|\?(?:[\w#!:\.\?\+=&%@!\-\/\(\)]+))?$");
        
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Matches(@"^\+7\d{10}$");
        
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Matches(@"^([A-Z]|[a-z]|[0-9])*$");
        
    }
}