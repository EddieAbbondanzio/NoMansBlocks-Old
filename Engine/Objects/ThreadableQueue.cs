using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Threadable Queue. Exactly as the name says
/// It provides a simple wrapper class for easier use of working with a queue that
/// can be safely multithreaded.
/// </summary>
public class ThreadableQueue<T> {
    #region Members
    /// <summary>
    /// The internal queue. Always call lock
    /// before accessing it.
    /// </summary>
    private Queue<T> queue;

    /// <summary>
    /// The semaphore lock object. It's readonly so a 
    /// new object can't replace it.
    /// </summary>
	private readonly object lockObj;
    #endregion

    #region Constructor(s)
    /// <summary>
    /// Create a new empty queue.
    /// </summary>
    public ThreadableQueue() {
        queue = new Queue<T>();
        lockObj = new object();
    }

    /// <summary>
    /// Create a new threadable queue with
    /// the contents of another queue.
    /// </summary>
    /// <param name="queue">The queue to copy in.</param>
    public ThreadableQueue(Queue<T> queue) {
        this.queue = queue;
        lockObj = new object();
    }

    /// <summary>
    /// Create a new threadable queue that has the
    /// contents of the list in it.
    /// </summary>
    /// <param name="values">The values to insert into
    /// the queue.</param>
    public ThreadableQueue(List<T> values) {
        this.queue = new Queue<T>(values);
        lockObj = new object();
    }
    #endregion

    #region Properties
    /// <summary>
    /// Returns total count of all objects in the queue.
    /// </summary>
    public int Count {
        get {
            lock (lockObj) {
                return queue.Count;
            }
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Enqueue the specified value.
    /// </summary>
    public void Enqueue(T value) {
		lock (lockObj) {
            queue.Enqueue(value);
        }
    }

    /// <summary>
    /// Enqueue the list of objects passed into
    /// the queue.
    /// </summary>
    /// <param name="values">The list to add
    /// to the queue.</param>
    public void Enqueue(List<T> values) {
        lock (lockObj) {
            values.ForEach(v => queue.Enqueue(v));
        }
    }

	/// <summary>
	/// Dequeue the specified value.
	/// </summary>
	public T Dequeue() {
		lock (lockObj) {
            return queue.Dequeue();
        }
    }

	/// <summary>
	/// Clear this instance of all values in it.
	/// </summary>
	public void Clear() {
		lock (lockObj) {
            queue.Clear();
        }
    }

    /// <summary>
    /// Checks if the specified value exists within the queue
    /// </summary>
    public bool Contains(T value){
		lock (lockObj) {
            return queue.Contains(value);
        }
    }

    /// <summary>
    /// Convert the queue's content into an array.
    /// </summary>
    /// <returns>The contents of the queue as an array.</returns>
    public T[] ToArray() {
        lock (lockObj) {
            return queue.ToArray();
        }
    }
    #endregion
}
