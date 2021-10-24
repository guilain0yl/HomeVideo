namespace ArgCheck.Core
{
    public enum RegexStrEnum
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        email_rule = 0,
        /// <summary>
        /// 手机号码
        /// </summary>
        phoneNumber_rule = 1,
        /// <summary>
        /// 汉字
        /// </summary>
        ch_ZN_rule = 2,
        /// <summary>
        /// 英文字母
        /// </summary>
        letter_rule = 5,
        /// <summary>
        /// 大写英文字母
        /// </summary>
        upper_letter_rule = 6,
        /// <summary>
        /// 小写英文字母
        /// </summary>
        lower_letter_rule = 7,
        /// <summary>
        /// 英文字母和数字
        /// </summary>
        letter_number_rule = 8,
        /// <summary>
        /// 英文字母、数字和下划线
        /// </summary>
        letter_number_underline_rule = 9,
        /// <summary>
        /// 汉字、英文字母、数字和下划线
        /// </summary>
        ch_ZN_letter_number_underline_rule = 10,
        /// <summary>
        /// 汉字、英文字母和数字
        /// </summary>
        ch_ZN_letter_number_rule = 11,
        /// <summary>
        /// 特殊符号（^%&’,;=?$\”）
        /// </summary>
        symbol_rule = 12,
        /// <summary>
        /// 域名
        /// </summary>
        domain_rule = 14,
        /// <summary>
        /// url
        /// </summary>
        url_rule = 15,
        /// <summary>
        /// 国内电话号码（xxxx-xxxxxxxx）
        /// </summary>
        telephoneNumber_rule = 17,
        /// <summary>
        /// 纯数字身份证
        /// </summary>
        idNumber_rule = 18,
        /// <summary>
        /// 含有x的短号身份证
        /// </summary>
        idNumber_x_rule = 19,
        /// <summary>
        /// 账号(字母开头，允许5-16字节，允许字母数字下划线)
        /// </summary>
        account_rule = 20,
        /// <summary>
        /// 密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)
        /// </summary>
        password_rule = 21,
        /// <summary>
        /// 强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间)
        /// </summary>
        strongPassword_rule = 22,
        /// <summary>
        /// xml
        /// </summary>
        xml_rule = 34,
        /// <summary>
        /// 邮政编码
        /// </summary>
        postCode_rule = 41,
        /// <summary>
        /// ip地址
        /// </summary>
        ip_rule = 42,
        /// <summary>
        /// ipv6地址
        /// </summary>
        ipv6_rule = 43
    }
}
