using UnityEngine;

namespace Training
{
    public class AtributeComponent : MonoBehaviour
    {
        [Header("Properties")]
        public float currentHP = 1000;
        public float maxHP;

        public float currentMP = 1000;
        public float maxMP;

        public float currentStamina;
        public float maxStamina;

        public float currentPosture;
        public float maxPosture;

        public float attack;
        public float defense;
        public float agility;
        public float inteligence;

        public float hyperArmor = 100;
        private void Awake()
        {
        }

        public void UpdateHealth(float amount)
        {
            currentHP += amount;
            hyperArmor -= 50.0f;
            Debug.Log("Current HP is: " + currentHP);
        }
        public void UpdateMana(float amount)
        {
            currentMP += amount;
            Debug.Log("Current MP is: " + currentMP);
        }
        public void UpdateStamina(float amount)
        {
            currentStamina += amount;
            Debug.Log("Current SP is: " + currentStamina);
        }
        //[Header("Health Bar UI")]
        //public Canvas UI;


        ////Test
        //float timeUpdated = 0.0f;
        //float timeDelayUpdated = 2.0f;

        //public void TakeDamage(float amount)
        //{
        //    currentHP -= amount;
        //    UpdateHealthUI();
        //}

        //private void Start()
        //{
        //    atributeDataTemporary = Instantiate(atributeDataOrigin);
        //    maxHP = atributeDataTemporary.data.baseHP;
        //    currentHP = maxHP;
        //}
        //private void Update()
        //{
        //    if(Time.time - timeUpdated > timeDelayUpdated)
        //    {
        //        TakeDamage(50);
        //        timeUpdated = Time.time;
        //    }
        //}
        //public void UpdateHealthUI()
        //{
        //    GameObject healthBarObject = UI.transform.Find("Health Bar").gameObject;
        //    float raito = currentHP / maxHP;
        //    healthBarObject.GetComponent<Slider>().value = raito;
        //}
    }
}
