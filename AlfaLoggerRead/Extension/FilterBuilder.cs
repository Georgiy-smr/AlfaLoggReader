using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlfaLoggerRead.Extension
{
    public static class FilterBuilder
    {
        /// <summary>
        /// Строит выражение, которое проверяет, что свойство (string) содержит хотя бы одно из значений.
        /// Пример: values = ["abc", "def"], propertySelector = x => x.EventPublishName
        /// Результат: x => x.EventPublishName.Contains("abc") || x.EventPublishName.Contains("def")
        /// </summary>
        public static Expression<Func<T, bool>> BuildContainsOrExpression<T>(this
            IEnumerable<string> values,
            Expression<Func<T, string>> propertySelector)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            var valueList = values.Where(v => !string.IsNullOrEmpty(v)).ToList();
            if (!valueList.Any())
                return x => false; // или x => true, если хотите пустой фильтр

            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;

            Expression? combinedExpression = null;

            foreach (var value in valueList)
            {
                // Строим выражение: x.EventPublishName.Contains(value)
                var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                var constant = Expression.Constant(value, typeof(string));
                var containsCall = Expression.Call(property, method, constant);

                if (combinedExpression == null)
                    combinedExpression = containsCall;
                else
                    combinedExpression = Expression.OrElse(combinedExpression, containsCall);
            }

            return Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter);
        }
    }


}
