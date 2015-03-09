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
    // Compacts the data so that it renders out quicker in the browser
    // This is a 2-pass solution where we first clean out the extra dates
    // then fill in the gaps
    // data: the 2D data array with time as the first index, and value as the second
    // secondInterval: the interval to determine if there is a "gap"
    self.compactTimeData = function (data, secondInterval)
    {
        // TODO: add this to JavaScript unit tests (like possibly QUnit?)
        for (var i = 0; i < data.length; i++)
        {
            var prevDataPoint = i >= 0 ? data[i - 1] : null,
                dataPoint = data[i],
                nextDataPoint = data[i + 1];

            if (prevDataPoint && prevDataPoint[0].diff(dataPoint[0], 's') > secondInterval)
            {
                // if there is a greater-than-interval time difference between the previous and
                // current data points, insert zero record at current position (to push the rest out by one)
                data.splice(i, 0, [dataPoint[0].add(-1, 's'), 0]);
            }
            else if (nextDataPoint && nextDataPoint[0].diff(dataPoint[0], 's') > secondInterval)
            {
                // if there is a greater-than-interval time difference between the current and
                // next data points, insert zero record at current position + 1 (to be processed next pass)
                data.splice(i + 1, 0, [dataPoint[0].add(1, 's'), 0]);
            }
            else if (prevDataPoint && nextDataPoint && prevDataPoint[1] == dataPoint[1] && dataPoint[1] == nextDataPoint[1])
            {
                // if the data is the same in all instances, remove it
                data.splice(i, 1);
                i--; // adjust i
            }
        }
    }

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