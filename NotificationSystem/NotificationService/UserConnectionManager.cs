namespace NotificationSystem.NotificationService
{
    public static class UserConnectionManager
    {
        private static readonly Dictionary<string, HashSet<string>> _connections = new();
        private static readonly object _lock = new();

        public static void AddConnection(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (!_connections.TryGetValue(userId, out var existing))
                {
                    existing = new HashSet<string>();
                    _connections[userId] = existing;
                }

                existing.Add(connectionId);
            }
        }

        public static void RemoveConnection(string userId, string connectionId)
        {
            lock (_lock)
            {
                if (_connections.TryGetValue(userId, out var existing))
                {
                    existing.Remove(connectionId);
                    if (existing.Count == 0)
                    {
                        _connections.Remove(userId);
                    }
                }
            }
        }

        // 🧠 Get total number of connected users
        public static List<string> GetConnectedUserCount()
        {
            lock (_lock)
            {
                return _connections.Select(x=>x.Key).ToList();
            }
        }

        // 🧠 Get total number of connections (across all users/tabs)
        public static int GetTotalConnectionCount()
        {
            lock (_lock)
            {
                return _connections.Values.Sum(set => set.Count);
            }
        }

        public static IReadOnlyCollection<string> GetConnections(string userId)
        {
            lock (_lock)
            {
                return _connections.TryGetValue(userId, out var existing) ? existing.ToList() : new List<string>();
            }
        }
    }

}
