/*
 * 
 * 游戏多线程
 * */
using UnityEngine;
using System.Threading;


public class BaseThread
{

    private static BaseThread instance;

    object obj = new object();
    int num = 0;
    private BaseThread()
    {

        /*测试线程优先级 
        /*/
        Thread th1 = new Thread(Th_test1);              //创建一个线程
        Thread th2 = new Thread(Th_test2);
        Thread th3 = new Thread(Th_test3);
        th1.Start();
        th2.Start();
        th3.Start();
        //学习优先级
        th1.Priority = System.Threading.ThreadPriority.Highest;         //优先级最高
        th2.Priority = System.Threading.ThreadPriority.Normal;
        th3.Priority = System.Threading.ThreadPriority.Lowest;
        //**/


        ///*测试线程锁

        /*/

        Thread th1 = new Thread(new ThreadStart(Th_lockTest));
        th1.Name = "test1";
        th1.Start();

        Thread th2 = new Thread(new ThreadStart(Th_lockTest));
        th2.Name = "test2";
        th2.Start();

		//*/




    }


    public static BaseThread GetInstance()
    {
        if (instance == null)
        {
            instance = new BaseThread();
        }
        return instance;
    }


    //测试多线程锁
    public void Th_lockTest()
    {

        Debug.Log("测试多线程");
        while (true)
        {
            lock (obj)
            {                                //线程“锁”         
                num++;
                Debug.Log(Thread.CurrentThread.Name + "测试多线程" + num);
            }
            Thread.Sleep(100);
            if (num > 300)
            {
                Thread.CurrentThread.Abort();
            }
        }
    }

    //测试多线程优先级
    public void Th_test1()
    {
        for (int i = 0; i < 500; i++)
        {

            Debug.Log("测试多线程1执行的次数:" + i);
            if (i > 200)
            {
                Thread.CurrentThread.Abort();
            }
        }
    }
    public void Th_test2()
    {
        for (int i = 0; i < 500; i++)
        {

            Debug.Log("测试多线程2执行的次数:" + i);
            if (i > 300)
            {
                Thread.CurrentThread.Abort();
            }
        }
    }
    public void Th_test3()
    {
        for (int i = 0; i < 500; i++)
        {

            Debug.Log("测试多线程3执行的次数:" + i);
            if (i > 400)
            {
                Thread.CurrentThread.Abort();
            }
        }
    }

}