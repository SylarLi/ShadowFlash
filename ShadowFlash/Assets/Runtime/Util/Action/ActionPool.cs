using System.Collections.Generic;

namespace Action
{
    public class ActionPool : IActionPool
    {
        private Dictionary<string, IActionInfo> _raws;

        public ActionPool()
        {
            _raws = new Dictionary<string, IActionInfo>();
        }

        public IActionInfo GetActionInfo(string raw)
        {
            if (!_raws.ContainsKey(raw) || _raws[raw] == null)
            {
                _raws[raw] = ProtoBufUtil.Deserialize<IActionInfo>(raw);
            }
            return _raws[raw].Clone();
        }
    }
}
