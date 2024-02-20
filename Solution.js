
//const {MinPriorityQueue} = require('@datastructures-js/priority-queue');
/**
 * @param {number} numberOfRooms
 * @param {number[][]} meetingIntervals
 * @return {number}
 */
var mostBooked = function (numberOfRooms, meetingIntervals) {

    //PriorityQueue<Integer>
    this.minHeapIndexFreeRooms = new MinPriorityQueue({compare: (x, y) => x - y});
    for (let i = 0; i < numberOfRooms; ++i) {
        this.minHeapIndexFreeRooms.enqueue(i);
    }

    //PriorityQueue<Room>
    this.minHeapEndTimeOccupiedRooms = new MinPriorityQueue({compare:
                (x, y) => x.endTimeMeeting === y.endTimeMeeting
                    ? x.index - y.index
                    : x.endTimeMeeting - y.endTimeMeeting});

    this.numberOfMeetingsPerRoom = new Array(numberOfRooms).fill(0);

    meetingIntervals.sort((x, y) => x[0] - y[0]);
    scheduleAllMeetings(meetingIntervals);

    return findSmallestIndexOfRoomWithMostMeetings(numberOfRooms);
};

/**
 * @param {number[][]} meetingIntervals
 * @return {void}
 */
function scheduleAllMeetings(meetingIntervals) {

    let roomNextMeeting;

    for (let [startNextMeeting, endNextMeeting] of meetingIntervals) {

        while (!this.minHeapEndTimeOccupiedRooms.isEmpty()
                && this.minHeapEndTimeOccupiedRooms.front().endTimeMeeting <= startNextMeeting) {
            this.minHeapIndexFreeRooms.enqueue(this.minHeapEndTimeOccupiedRooms.dequeue().index);
        }

        if (!this.minHeapIndexFreeRooms.isEmpty()) {
            roomNextMeeting = new Room(endNextMeeting, this.minHeapIndexFreeRooms.dequeue());
        } else {
            roomNextMeeting = this.minHeapEndTimeOccupiedRooms.dequeue();
            roomNextMeeting.endTimeMeeting += endNextMeeting - startNextMeeting;
        }

        this.minHeapEndTimeOccupiedRooms.enqueue(roomNextMeeting);
        ++this.numberOfMeetingsPerRoom[roomNextMeeting.index];
    }
}

/**
 * @param {number} numberOfRooms
 * @return {number}
 */
function findSmallestIndexOfRoomWithMostMeetings(numberOfRooms) {
    let indexOfRoomWithMostMeetings = 0;
    let maxNumberOfMeetingsPerRoom = 0;

    for (let i = 0; i < numberOfRooms; ++i) {
        if (maxNumberOfMeetingsPerRoom < this.numberOfMeetingsPerRoom[i]) {
            maxNumberOfMeetingsPerRoom = this.numberOfMeetingsPerRoom[i];
            indexOfRoomWithMostMeetings = i;
        }
    }
    return indexOfRoomWithMostMeetings;
}

/**
 * @param {number} endTimeOfMeeting
 * @param {number} index
 */
function Room(endTimeOfMeeting, index) {
    this.endTimeMeeting = endTimeOfMeeting;
    this.index = index;
}
