### Threading in C#

Modern Applications Need To Deal With Running Different Units Out-Of-Order Or At The Same Time Simultaneously Without Affecting
The Outcome Result
Threading in C# allows you to execute multiple tasks concurrently, enhancing the responsiveness and efficiency of your applications. Here are the key concepts related to threading:

#### 1. Process vs Thread

- **Process**: A process is an instance of a running program on a computer. It consists of its own memory space, resources, and a single thread of execution.
  
- **Thread**: A thread is the smallest unit of execution within a process. Threads share the process's resources but run independently. They allow for concurrent execution of tasks within the same program.

#### 2. Start and Join Thread

- **Starting a Thread**: In C#, you start a thread by creating an instance of the `Thread` class and passing a method to its constructor that represents the code to be executed by the thread. You then call `Start()` method on the thread instance to begin execution.

  ```csharp
  Thread thread = new Thread(SomeMethod);
  thread.Start();
  ```

- **Joining a Thread**: The `Join()` method blocks the calling thread until the thread on which it is called completes its execution. This ensures that the main thread waits for the other thread to finish before proceeding.

  ```csharp
  thread.Join(); // Wait for 'thread' to complete
  ```

#### 3. Multi-Threading

- **Multi-Threading**: Multi-threading refers to the concurrent execution of multiple threads within the same process. It allows applications to perform multiple tasks simultaneously, improving performance and responsiveness.

  ```csharp
  Thread thread1 = new Thread(Method1);
  Thread thread2 = new Thread(Method2);

  thread1.Start();
  thread2.Start();

  thread1.Join();
  thread2.Join();
  ```

#### 4. Race Condition

- **Race Condition**: A race condition occurs when multiple threads access shared data or resources concurrently, and the outcome depends on the timing of their execution. It can lead to unpredictable results or incorrect behavior of the program.

  ```csharp
  // Example of a race condition
  int counter = 0;

  void IncrementCounter()
  {
      for (int i = 0; i < 1000; i++)
      {
          counter++;
      }
  }
  ```

#### 5. Deadlock

- **Deadlock**: Deadlock is a situation where two or more threads are blocked forever, waiting for each other to release resources that they need. It typically occurs when threads acquire locks in a circular manner.

  ```csharp
  object lock1 = new object();
  object lock2 = new object();

  void Thread1()
  {
      lock (lock1)
      {
          Thread.Sleep(100);
          lock (lock2)
          {
              // Do something
          }
      }
  }

  void Thread2()
  {
      lock (lock2)
      {
          Thread.Sleep(100);
          lock (lock1)
          {
              // Do something
          }
      }
  }
  ```

#### 6. Thread Pool

- **Thread Pool**: A thread pool is a collection of pre-created threads that are available for performing tasks asynchronously. It helps reduce the overhead of creating new threads for short-lived tasks and manages the number of threads efficiently.

  ```csharp
  ThreadPool.QueueUserWorkItem(Method);
  
  void Method(object state)
  {
      // Execute task asynchronously
  }
  ```

### Conclusion

Threading in C# is a powerful mechanism for achieving concurrency and parallelism in applications. Understanding these concepts—process vs thread, starting and joining threads, multi-threading, race conditions, deadlock, and thread pool—enables developers to write efficient and responsive multi-threaded applications while avoiding common pitfalls like race conditions and deadlocks. Each concept plays a crucial role in designing robust concurrent systems in C#.