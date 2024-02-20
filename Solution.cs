
using System;
using System.Collections.Generic;

public class Solution
{
    private PriorityQueue<int, int>? minHeapIndexFreeRooms;
    private PriorityQueue<Room, Room>? minHeapEndTimeOccupiedRooms;
    private int[]? numberOfMeetingsPerRoom;
    public int MostBooked(int numberOfRooms, int[][] meetingIntervals)
    {
        minHeapIndexFreeRooms = new PriorityQueue<int, int>();
        for (int i = 0; i < numberOfRooms; ++i)
        {
            minHeapIndexFreeRooms.Enqueue(i, i);
        }

        Comparer<Room> compareRooms = Comparer<Room>.Create(
                (x, y) => x.endTimeMeeting.CompareTo(y.endTimeMeeting) == 0
                        ? x.index.CompareTo(y.index)
                        : x.endTimeMeeting.CompareTo(y.endTimeMeeting));

        minHeapEndTimeOccupiedRooms = new PriorityQueue<Room, Room>(compareRooms);

        Array.Sort(meetingIntervals, (x, y) => x[0] - y[0]);
        ScheduleAllMeetings(numberOfRooms, meetingIntervals);

        return FindSmallestIndexOfRoomWithMostMeetings(numberOfRooms);
    }

    private void ScheduleAllMeetings(int numberOfRooms, int[][] meetingIntervals)
    {
        numberOfMeetingsPerRoom = new int[numberOfRooms];
        Room roomNextMeeting;

        foreach (int[] interval in meetingIntervals)
        {
            int startNextMeeting = interval[0];
            int endNextMeeting = interval[1];

            while (minHeapEndTimeOccupiedRooms.Count > 0
                    && minHeapEndTimeOccupiedRooms.Peek().endTimeMeeting <= startNextMeeting)
            {
                int index = minHeapEndTimeOccupiedRooms.Dequeue().index;
                minHeapIndexFreeRooms.Enqueue(index, index);
            }

            if (minHeapIndexFreeRooms.Count > 0)
            {
                roomNextMeeting = new Room(endNextMeeting, minHeapIndexFreeRooms.Dequeue());
            }
            else
            {
                roomNextMeeting = minHeapEndTimeOccupiedRooms.Dequeue();
                roomNextMeeting.endTimeMeeting += endNextMeeting - startNextMeeting;
            }

            minHeapEndTimeOccupiedRooms.Enqueue(roomNextMeeting, roomNextMeeting);
            ++numberOfMeetingsPerRoom[roomNextMeeting.index];
        }
    }

    private int FindSmallestIndexOfRoomWithMostMeetings(int numberOfRooms)
    {
        int indexOfRoomWithMostMeetings = 0;
        int maxNumberOfMeetingsPerRoom = 0;

        for (int i = 0; i < numberOfRooms; ++i)
        {
            if (maxNumberOfMeetingsPerRoom < numberOfMeetingsPerRoom[i])
            {
                maxNumberOfMeetingsPerRoom = numberOfMeetingsPerRoom[i];
                indexOfRoomWithMostMeetings = i;
            }
        }
        return indexOfRoomWithMostMeetings;
    }
}

class Room
{
    public long endTimeMeeting;
    public int index;

    public Room()
    {
    }

    public Room(int endTimeOfMeeting, int index)
    {
        this.endTimeMeeting = endTimeOfMeeting;
        this.index = index;
    }
}
