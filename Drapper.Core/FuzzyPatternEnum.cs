namespace Drapper.Core
{
    public enum FuzzyPatternEnum
    {
        /// <summary>
        /// 全模糊查询 %a%
        /// </summary>
        ALL = 1,
        /// <summary>
        /// 匹配前面 %a
        /// </summary>
        Forward = 2,
        /// <summary>
        /// 匹配后面 a%
        /// </summary>
        Backward = 3
    }
}
