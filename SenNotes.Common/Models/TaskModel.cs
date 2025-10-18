using Newtonsoft.Json;

namespace SenNotes.Common.Models
{
    public class TaskModel
    {
        [JsonIgnore] public int Id { get; set; }
        [JsonProperty("start_time")] public DateTime? StartTime { get; set; }

        [JsonProperty("end_time")] public DateTime? EndTime { get; set; }

        [JsonProperty("task_title")] public string? TaskTitle { get; set; }

        [JsonProperty("task_detail")] public string? TaskDetail { get; set; }

        [JsonIgnore] public List<FileSource>? Files { get; set; }
        
        [JsonIgnore]
        public string? InputMessage { get; set; }

        [JsonIgnore]
        public double Progress
        {
            get
            {
                //  情况 1：没有开始时间、没有结束时间 → 无法计算进度
                if (StartTime is null && EndTime is null)
                    return 0;

                //  情况 2：有结束时间但没有开始时间 → 默认认为任务未开始
                if (StartTime is null && EndTime is not null)
                {
                    return GetReverseProgressByDays((EndTime.Value-DateTime.Now).TotalDays);
                }

                //  情况 3：有开始时间但没有结束时间 → 无法计算终点，显示0或进行中
                if (StartTime is not null && EndTime is null)
                {
                    if (DateTime.Now < StartTime.Value)
                        return 0; // 还没开始
                    return 50;    // 任务正在进行（可根据你需要改为 0 或动态值）
                }

                //  情况 4：开始时间与结束时间都有 → 正常计算进度
                if (StartTime is not null && EndTime is not null)
                {
                    var total = EndTime.Value - StartTime.Value;
                    var elapsed = DateTime.Now - StartTime.Value;

                    if (elapsed.TotalSeconds <= 0)
                        return 0;   // 还没开始
                    if (elapsed >= total)
                        return 100; // 已结束

                    // 按比例计算百分比
                    return (elapsed.TotalSeconds / total.TotalSeconds) * 100;
                }

                return 0; // 理论上不会到这里
            }
        }
        
        private static double GetReverseProgressByDays(double diffDays)
        {
            // 大于等于30天 => 0
            if (diffDays >= 30)
                return 0;

            // 小于等于0天 => 100
            if (diffDays <= 0)
                return 100;

            // 否则线性计算，越接近0越大
            return (1 - diffDays / 30.0) * 100;
        }
        
        [JsonIgnore]
        public TaskStatus Status
        {
            get
            {
                switch (Progress)
                {
                    case double and >= 0 and <= 20:
                        return TaskStatus.Easy;
                    case double and > 20 and <= 50:
                        return TaskStatus.Notice;
                    case double and > 50 and < 100:
                        return TaskStatus.Emergency;
                    case double and >= 100:
                        return TaskStatus.Overdue;
                    default:
                        return TaskStatus.Unknown;
                }
            }
        }
    }
}