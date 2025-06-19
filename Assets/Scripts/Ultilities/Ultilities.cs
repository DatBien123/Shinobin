using UnityEngine;

    public class Utilities : MonoBehaviour
    {
        // Instance tĩnh để truy cập Singleton
        private static Utilities _instance;

        // Thuộc tính để lấy instance
        public static Utilities Instance
        {
            get
            {
                // Nếu chưa có instance, tìm hoặc tạo mới
                if (_instance == null)
                {
                    // Tìm trong scene
                    _instance = FindObjectOfType<Utilities>();

                    // Nếu không tìm thấy, tự động tạo mới
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("Utilities");
                        _instance = singletonObject.AddComponent<Utilities>();
                        DontDestroyOnLoad(singletonObject); // Giữ instance tồn tại giữa các scene
                    }
                }
                return _instance;
            }
        }

        // Đảm bảo không tạo thêm instance khi đã có sẵn
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); // Hủy bỏ object trùng lặp
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject); // Giữ object không bị phá hủy khi load scene mới
            }
        }

        public float DistanceCalculate(Vector3 A, Vector3 B, bool ignoreY = false)
        {
            Vector3 a = A;
            Vector3 b = B;
            if (ignoreY)
            {
                a.y = b.y = 0;
            }
            return Vector3.Distance(a, b);
        }

        public float HeightDiff(Vector3 A, Vector3 B)
        {
            return Mathf.Abs(A.y - B.y);
        }
    }