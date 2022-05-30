using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public static CanvasUI canvas = null;
    private void Awake()
    {
        if (canvas == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������ 
        {
            canvas = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ���� 
        }
        else
        {
            if (canvas != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ� 
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ���� 
        }
    }
}
