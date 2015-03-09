/*! SACS Common.js
 * ======================
 * The file containing the common javascript used for SACS.
 * This includes the helpers
 *
 * @Author  Chimbala Wright
 * @version 1.0.0.0
 */

// The GraphHelper function
(function (self)
{
    // Takes JSON data array from a server, returning a 2D array mapping the first parameter as audit
    // time to a normalized time value and the second parameter to the value.
    self.mapAuditTimeTo2DArray = function (data)
    {
        var newArray = [];

        if (data)
        {
            $.map(data, function (el, i)
            {
                var date = moment(el.AuditTime);
                date = date.add(date.utcOffset(), 'm');
                newArray.push([date, el.Value]);
            });
        }

        return newArray;
    };

})(window.GraphHelper = window.GraphHelper || {});