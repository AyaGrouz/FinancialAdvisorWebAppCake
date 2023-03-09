import { HttpClient } from '@angular/common/http';
import { Component, OnInit, OnDestroy, EventEmitter, Output, Input } from '@angular/core';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { NamedRoom, VideoChatService } from '../services/videochat.service';


@Component({
    selector: 'app-rooms',
    styleUrls: ['./rooms.component.css'],
    templateUrl: './rooms.component.html',
})
export class RoomsComponent implements OnInit, OnDestroy {
    @Output() roomChanged = new EventEmitter<string>();
    @Output() startQuiz = new EventEmitter<string>();

    @Input() activeRoomName: string;

    roomName: string;
    rooms: NamedRoom[];

    private subscription: Subscription;

    constructor(
        private readonly videoChatService: VideoChatService, private http: HttpClient) { }

    async ngOnInit() {
        await this.updateRooms();
        this.subscription =
            this.videoChatService
                .$roomsUpdated
                .pipe(tap(_ => this.updateRooms()))
                .subscribe();
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    onTryAddRoom() {
        if (this.roomName) {
            this.onAddRoom(this.roomName);
        }
    }

    onAddRoom(roomName: string) {
        this.roomName = null;
        var lastRoomName = "";
        this.http.get('/api/video/lastRoom').subscribe((result) => {
            lastRoomName = result["lastRoomName"];
        }, error => console.error(error));
        setTimeout(() => {
            if (!(lastRoomName.toLowerCase()).includes("room")) {
                roomName = "Room_0";
            }
            else {
                const chars = lastRoomName.split('_');
                var number = parseInt(chars[1]);
                lastRoomName = lastRoomName.replace(/[0-9]/g, '');
                roomName = lastRoomName + (number + 1);
            }
            this.roomChanged.emit(roomName);
            setTimeout(() => {
                this.startQuiz.emit();
            }, 4000);
        }, 1500);


    }

    onJoinRoom(roomName: string) {
        this.roomChanged.emit(roomName);
    }

    async updateRooms() {
        this.rooms = (await this.videoChatService.getAllRooms()) as NamedRoom[];
    }
}