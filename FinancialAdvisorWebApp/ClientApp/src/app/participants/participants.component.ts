import { HttpClient } from '@angular/common/http';
import {
    Component,
    ViewChild,
    ElementRef,
    Output,
    Input,
    EventEmitter,
    Renderer2
} from '@angular/core';
import { Router } from '@angular/router';
import { Room } from 'twilio-video';
import {
    Participant,
    RemoteTrack,
    RemoteAudioTrack,
    RemoteVideoTrack,
    RemoteParticipant,
    RemoteTrackPublication
} from 'twilio-video';
import { UserService } from '../shared/user.service';

@Component({
    selector: 'app-participants',
    styleUrls: ['./participants.component.css'],
    templateUrl: './participants.component.html',
})
export class ParticipantsComponent {
    @ViewChild('list') listRef: ElementRef;
    @Output('participantsChanged') participantsChanged = new EventEmitter<boolean>();
    @Output('leaveRoom') leaveRoom = new EventEmitter<boolean>();
    @Input('activeRoom') activeRoom: Room;

    public id_invest;
    public userDetails;

    get participantCount() {
        return !!this.participants ? this.participants.size : 0;
    }

    get isAlone() {
        return this.participantCount === 0;
    }

    private participants: Map<Participant.SID, RemoteParticipant>;
    private dominantSpeaker: RemoteParticipant;

    constructor(private readonly renderer: Renderer2, private http: HttpClient, private service: UserService, private router: Router) { }
    async ngOnInit() {
        this.service.getUserProfile().subscribe(
            res => {
                this.userDetails = res;
                this.id_invest = this.userDetails['id'];
            },
            err => {
                console.log(err);
            },
        );
    }

    clear() {
        if (this.participants) {
            this.participants.clear();
        }
    }

    initialize(participants: Map<Participant.SID, RemoteParticipant>) {
        this.participants = participants;
        if (this.participants) {
            this.participants.forEach(participant => this.registerParticipantEvents(participant));
        }
    }


    add(participant: RemoteParticipant) {
        if (this.participants && participant) {
            this.participants.set(participant.sid, participant);
            this.registerParticipantEvents(participant);
        }
    }

    remove(participant: RemoteParticipant) {
        if (this.participants && this.participants.has(participant.sid)) {
            this.participants.delete(participant.sid);
        }
    }

    loudest(participant: RemoteParticipant) {
        this.dominantSpeaker = participant;
    }

    onLeaveRoom() {
        this.leaveRoom.emit(true);
    }
    onCompleteRoom() {

        var activeRoom = this.activeRoom.sid;
        this.http.post('/api/video/', { "Id_invest": this.id_invest, "version": 0, "answer": null, "RoomSid": activeRoom }).subscribe(
            result => {
                console.log(result);
                alert("Results ready !");
                window.location.reload();
            });
        setTimeout(() => {
            alert("This operation may take several minutes !");
            this.router.navigate(['/investisseur'])
                .then(() => {
                    window.location.reload();
                });
            //this.router.navigateByUrl('/investisseur');
        }, 3000);

    }

    private registerParticipantEvents(participant: RemoteParticipant) {
        if (participant) {
            participant.tracks.forEach(publication => this.subscribe(publication));
            participant.on('trackPublished', publication => this.subscribe(publication));
            participant.on('trackUnpublished',
                publication => {
                    if (publication && publication.track) {
                        this.detachRemoteTrack(publication.track);
                    }
                });
        }
    }

    private subscribe(publication: RemoteTrackPublication | any) {
        if (publication && publication.on) {
            publication.on('subscribed', track => this.attachRemoteTrack(track));
            publication.on('unsubscribed', track => this.detachRemoteTrack(track));
        }
    }

    private attachRemoteTrack(track: RemoteTrack) {
        if (this.isAttachable(track)) {
            const element = track.attach();
            this.renderer.data.id = track.sid;
            this.renderer.setStyle(element, 'width', '95%');
            this.renderer.setStyle(element, 'margin-left', '2.5%');
            this.renderer.appendChild(this.listRef.nativeElement, element);
            this.participantsChanged.emit(true);
        }
    }

    private detachRemoteTrack(track: RemoteTrack) {
        if (this.isDetachable(track)) {
            track.detach().forEach(el => el.remove());
            this.participantsChanged.emit(true);
        }
    }

    private isAttachable(track: RemoteTrack): track is RemoteAudioTrack | RemoteVideoTrack {
        return !!track &&
            ((track as RemoteAudioTrack).attach !== undefined ||
                (track as RemoteVideoTrack).attach !== undefined);
    }

    private isDetachable(track: RemoteTrack): track is RemoteAudioTrack | RemoteVideoTrack {
        return !!track &&
            ((track as RemoteAudioTrack).detach !== undefined ||
                (track as RemoteVideoTrack).detach !== undefined);
    }
}