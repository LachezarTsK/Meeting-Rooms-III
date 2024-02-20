
import java.util.Arrays;
import java.util.PriorityQueue;

public class Solution {

    private PriorityQueue<Integer> minHeapIndexFreeRooms;
    private PriorityQueue<Room> minHeapEndTimeOccupiedRooms;
    private int[] numberOfMeetingsPerRoom;

    public int mostBooked(int numberOfRooms, int[][] meetingIntervals) {

        minHeapIndexFreeRooms = new PriorityQueue<>();
        for (int i = 0; i < numberOfRooms; ++i) {
            minHeapIndexFreeRooms.add(i);
        }

        minHeapEndTimeOccupiedRooms = new PriorityQueue<>(
                (x, y) -> Long.compare(x.endTimeMeeting, y.endTimeMeeting) == 0
                         ? x.index - y.index
                         : Long.compare(x.endTimeMeeting, y.endTimeMeeting));

        Arrays.sort(meetingIntervals, (x, y) -> x[0] - y[0]);
        scheduleAllMeetings(numberOfRooms, meetingIntervals);

        return findSmallestIndexOfRoomWithMostMeetings(numberOfRooms);
    }

    private void scheduleAllMeetings(int numberOfRooms, int[][] meetingIntervals) {

        numberOfMeetingsPerRoom = new int[numberOfRooms];
        Room roomNextMeeting;

        for (int[] interval : meetingIntervals) {
            int startNextMeeting = interval[0];
            int endNextMeeting = interval[1];

            while (!minHeapEndTimeOccupiedRooms.isEmpty()
                    && minHeapEndTimeOccupiedRooms.peek().endTimeMeeting <= startNextMeeting) {
                minHeapIndexFreeRooms.add(minHeapEndTimeOccupiedRooms.poll().index);
            }

            if (!minHeapIndexFreeRooms.isEmpty()) {
                roomNextMeeting = new Room(endNextMeeting, minHeapIndexFreeRooms.poll());
            } else {
                roomNextMeeting = minHeapEndTimeOccupiedRooms.poll();
                roomNextMeeting.endTimeMeeting += endNextMeeting - startNextMeeting;
            }

            minHeapEndTimeOccupiedRooms.add(roomNextMeeting);
            ++numberOfMeetingsPerRoom[roomNextMeeting.index];
        }
    }

    private int findSmallestIndexOfRoomWithMostMeetings(int numberOfRooms) {
        int indexOfRoomWithMostMeetings = 0;
        int maxNumberOfMeetingsPerRoom = 0;

        for (int i = 0; i < numberOfRooms; ++i) {
            if (maxNumberOfMeetingsPerRoom < numberOfMeetingsPerRoom[i]) {
                maxNumberOfMeetingsPerRoom = numberOfMeetingsPerRoom[i];
                indexOfRoomWithMostMeetings = i;
            }
        }
        return indexOfRoomWithMostMeetings;
    }
}

class Room {

    long endTimeMeeting;
    int index;

    Room() {
    }

    Room(int endTimeOfMeeting, int index) {
        this.endTimeMeeting = endTimeOfMeeting;
        this.index = index;
    }
}
