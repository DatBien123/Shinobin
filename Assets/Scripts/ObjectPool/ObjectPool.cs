using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject clonesParent;
    public GameObject clonePrefab; // Prefab của clone.
    public int poolSize = 10; // Số lượng clone trong pool.

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    void Start()
    {
        // Khởi tạo pool với số lượng clone ban đầu.
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(clonePrefab);
            obj.transform.SetParent(clonesParent.transform, false);
            obj.SetActive(false); // Đặt trạng thái ban đầu là không kích hoạt.
            poolQueue.Enqueue(obj);
        }

    }

    // Lấy một object từ pool.
    public GameObject GetFromPool()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true); // Kích hoạt object.
            return obj;
        }
        else
        {
            // Nếu pool hết object, tạo thêm (tùy thuộc vào yêu cầu).
            GameObject obj = Instantiate(clonePrefab);
            obj.SetActive(true);
            return obj;
        }
    }

    // Trả object về pool.
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false); // Vô hiệu hóa object.
        poolQueue.Enqueue(obj);
    }
}