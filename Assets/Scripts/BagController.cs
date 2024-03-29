using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagController : MonoBehaviour
{
    [SerializeField] private Transform bag;
    public List<ProductData> productDataList;
    private Vector3 productSize;
    [SerializeField] private TextMeshPro maxText;
    int maxBagCapacity;
    // Start is called before the first frame update
    void Start()
    {
        maxBagCapacity = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShopPoint"))
        {
            PlayShopSound();
            for (int i = productDataList.Count - 1; i >= 0 ; i--)
            {
                SellProductsToShop(productDataList[i]);
                Destroy(bag.transform.GetChild(i).gameObject);
                productDataList.RemoveAt(i);
            }
            ControlBagCapacity();
        }

        if (other.CompareTag("UnlockBakeryUnit"))
        {   
            UnlockBakeryUnitController bakeryUnit = other.GetComponent<UnlockBakeryUnitController>();

            ProductType neededType = bakeryUnit.GetNeededProductType();

            for (int i = productDataList.Count - 1; i >= 0; i--)
            {
                if (productDataList[i].productType == neededType)
                {
                    if (bakeryUnit.StoredProduct() == true)
                    {
                        Destroy(bag.transform.GetChild(i).gameObject);
                        productDataList.RemoveAt(i);
                    }
                }
            }
            StartCoroutine(PutProductsInOrder());
            ControlBagCapacity();
        }
    }

    private void SellProductsToShop(ProductData productData)
    {
        CashManager.instance.ExchangeProduct(productData);
        //Cash manager'a s�yle �r�nler sat�ld�
    }

    public void AddProductToBag(ProductData productData)
    {
        GameObject boxProduct = Instantiate(productData.productPrefab, Vector3.zero, Quaternion.identity);
        boxProduct.transform.SetParent(bag, true);

        CalculateObjectSize(boxProduct);
        float yPosition = CalculateNewYPositionOfBox();
        boxProduct.transform.localRotation = Quaternion.identity;
        boxProduct.transform.localPosition = Vector3.zero;
        boxProduct.transform.localPosition = new Vector3(0, yPosition, 0);
        productDataList.Add(productData);
        ControlBagCapacity();
    }

    private float CalculateNewYPositionOfBox()
    {
        float newYPos = productSize.y * productDataList.Count;
        return newYPos;
    }

    private void CalculateObjectSize(GameObject gameObject)
    {
        if(productSize == Vector3.zero)
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            productSize = renderer.bounds.size;
        }
        
    }

    private void ControlBagCapacity()
    {
        if (productDataList.Count == maxBagCapacity)
        {
            SetMaxTextOn();
            //max yaz�s�n� g�ster ve daha fazla �r�n al�nmas�n� �nle
        }
        else
        {
            SetMaxTextOff();
        }
    }

    private void SetMaxTextOn()
    {
        if (!maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(true);
        }
        
    }

    private void SetMaxTextOff()
    {
        if (maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(false);
        }
    }
    public bool IsEmptySpace()
    {
        if (productDataList.Count < maxBagCapacity)
        {
            return true;
        }
        return false;
    }

    private IEnumerator PutProductsInOrder()
    {
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < bag.childCount; i++) 
        {
            float newYpos = productSize.y * i;
            bag.GetChild(i).transform.localPosition = new Vector3(0, newYpos, 0);
        }
    }

    private void PlayShopSound()
    {
        if (productDataList.Count > 0)
        {
            AudioManager.instance.PlayAudio(AudioClipType.shopClip);
            //AudioManager.instance.StopBackgroundMusic(); //Background m�zi�inin s�rekli devap etmesi i�in bu sat�r� silebilirsin
        }
    }
}
