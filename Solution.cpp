
#include <span>
#include <queue>
#include <ranges>
#include <vector>
using namespace std;

class Solution {

    struct Room {
        long long endTimeMeeting{};
        int index{};

        Room() = default;

        Room(int endTimeOfMeeting, int index) :
        endTimeMeeting{endTimeOfMeeting}, index{index}{}
    };

    struct CompareRooms {
        
        auto operator()(const Room& first, const Room& second) const {
            return first.endTimeMeeting == second.endTimeMeeting
                    ? first.index > second.index
                    : first.endTimeMeeting > second.endTimeMeeting;
        }
    };

    priority_queue<int, vector<int>, greater<>> minHeapIndexFreeRooms;
    priority_queue<Room, vector<Room>, CompareRooms> minHeapEndTimeOccupiedRooms;
    vector<int> numberOfMeetingsPerRoom;

public:
    int mostBooked(int numberOfRooms, vector<vector<int>>& meetingIntervals) {

        for (int i = 0; i < numberOfRooms; ++i) {
            minHeapIndexFreeRooms.push(i);
        }
  
        ranges::sort(meetingIntervals, [](const auto& x, const auto& y) {
                                          return x[0] < y[0]; });

        scheduleAllMeetings(numberOfRooms, meetingIntervals);

        return findSmallestIndexOfRoomWithMostMeetings(numberOfRooms);
    }

private:
    void scheduleAllMeetings(int numberOfRooms, span<const vector<int>> meetingIntervals) {

        numberOfMeetingsPerRoom.resize(numberOfRooms);
        Room roomNextMeeting;

        for (const auto& interval : meetingIntervals) {
            int startNextMeeting = interval[0];
            int endNextMeeting = interval[1];

            while (!minHeapEndTimeOccupiedRooms.empty()
                    && minHeapEndTimeOccupiedRooms.top().endTimeMeeting <= startNextMeeting) {

                int index = minHeapEndTimeOccupiedRooms.top().index;
                minHeapEndTimeOccupiedRooms.pop();
                minHeapIndexFreeRooms.push(index);
            }

            if (!minHeapIndexFreeRooms.empty()) {
                int index = minHeapIndexFreeRooms.top();
                minHeapIndexFreeRooms.pop();
                roomNextMeeting = Room(endNextMeeting, index);
            } else {
                roomNextMeeting = minHeapEndTimeOccupiedRooms.top();
                minHeapEndTimeOccupiedRooms.pop();
                roomNextMeeting.endTimeMeeting += endNextMeeting - startNextMeeting;
            }

            minHeapEndTimeOccupiedRooms.push(roomNextMeeting);
            ++numberOfMeetingsPerRoom[roomNextMeeting.index];
        }
    }

    int findSmallestIndexOfRoomWithMostMeetings(int numberOfRooms) const {
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
};
