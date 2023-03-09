import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { createLocalAudioTrack, Room, LocalTrack, LocalVideoTrack, LocalAudioTrack, RemoteParticipant } from 'twilio-video';
import { RoomsComponent } from '../rooms/rooms.component';
import { CameraComponent } from '../camera/camera.component';
import { SettingsComponent } from '../settings/settings.component';
import { ParticipantsComponent } from '../participants/participants.component';
import { VideoChatService } from '../services/videochat.service';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { UserService } from './../shared/user.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
declare const speechUtteranceChunker: any;
declare var anime: any;

@Component({
  selector: 'app-video-call',
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css']
})
export class VideoCallComponent implements OnInit, AfterViewInit {
  @ViewChild('rooms') rooms: RoomsComponent;
  @ViewChild('camera') camera: CameraComponent;
  @ViewChild('settings') settings: SettingsComponent;
  @ViewChild('participants') participants: ParticipantsComponent;

  activeRoom: Room;


  private notificationHub: HubConnection;
  public userDetails;
  _beep = new window.Audio("assets/beep.wav");
  synth = window.speechSynthesis;
  Questions;
  public id_invest;
  public risk = 0;
  txt = "";
  /*   answer = [
      { "Id_question": 1, "Choice": null },
      { "Id_question": 2, "Choice": null },
      { "Id_question": 3, "Choice": null },
      { "Id_question": 4, "Choice": null },
      { "Id_question": 5, "Choice": null },
      { "Id_question": 6, "Choice": null },
      { "Id_question": 7, "Choice": null },
      { "Id_question": 8, "Choice": null },
      { "Id_question": 9, "Choice": null },
      { "Id_question": 10, "Choice": null },
      { "Id_question": 11, "Choice": null },
      { "Id_question": 12, "Choice": null }
    ]; */
  answer = [
    { "Id_question": 1, "Choice": "Between 18 and 40 years" },
    { "Id_question": 2, "Choice": "Less than 25,00$" },
    { "Id_question": 3, "Choice": "In more than 20 years" },
    { "Id_question": 4, "Choice": "Very good : Barely or no debt and a lot of savings" },
    { "Id_question": 5, "Choice": "Development and Security : I’m looking for a balance between security and development to get a return slightly above inflation" },
    { "Id_question": 6, "Choice": "Beginner : I know that there is different type of investment but i don’t know what is the difference between them" },
    { "Id_question": 7, "Choice": "More than 10" },
    { "Id_question": 8, "Choice": "More than 20%" },
    { "Id_question": 9, "Choice": "From - 2,00$ to + 2,50$" },
    { "Id_question": 10, "Choice": "I would keep my investment because we expect market fluctuations. It’s the long-growth of this investment that interests me and the fluctuations in short term do  not worry me " },
    { "Id_question": 11, "Choice": "A good return, with reasonable security for your invested capital" },
    { "Id_question": 12, "Choice": null }
  ];


  constructor(private router: Router, private service: UserService,
    private readonly videoChatService: VideoChatService, private http: HttpClient) { }
  ngAfterViewInit(): void {
    const textWrapper = document.getElementById('an-1');
    textWrapper.innerHTML = textWrapper.textContent.replace(/\S/g, "<span class='letter'>$&</span>");

    anime.timeline({ loop: false })
      .add({
        targets: '.an-1 .letter',
        scale: [4, 1],
        opacity: [0, 1],
        translateZ: 0,
        easing: "easeOutExpo",
        duration: 950,
        delay: (el, i) => 100 * i
      });
  }

  async ngOnInit() {
    this.http.get('/api/GetQuestionList/' + this.id_invest).subscribe((result) => {
      this.Questions = result["listQuestions"];
    }, error => console.error(error));

    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
        this.id_invest = this.userDetails['id'];
      },
      err => {
        console.log(err);
      },
    );

    const builder =
      new HubConnectionBuilder()
        .configureLogging(LogLevel.Information)
        .withUrl(`${location.origin}/notificationHub`);

    this.notificationHub = builder.build();
    this.notificationHub.on('RoomsUpdated', async updated => {
      if (updated) {
        await this.rooms.updateRooms();
      }
    });

    await this.notificationHub.start();
  }

  async onSettingsChanged(deviceInfo?: MediaDeviceInfo) {
    await this.camera.initializePreview(deviceInfo.deviceId);
    if (this.settings.isPreviewing) {
      const track = await this.settings.showPreviewCamera();
      if (this.activeRoom) {
        const localParticipant = this.activeRoom.localParticipant;
        localParticipant.videoTracks.forEach(publication => publication.unpublish());
        await localParticipant.publishTrack(track);
      }
    }
  }

  async onLeaveRoom(_: boolean) {
    if (this.activeRoom) {
      this.activeRoom.disconnect();
      this.activeRoom = null;
    }

    const videoDevice = this.settings.hidePreviewCamera();
    await this.camera.initializePreview(videoDevice && videoDevice.deviceId);

    this.participants.clear();
  }

  async onRoomChanged(roomName: string) {
    if (roomName) {
      if (this.activeRoom) {
        this.activeRoom.disconnect();
      }

      this.camera.finalizePreview();

      const tracks = await Promise.all([
        createLocalAudioTrack(),
        this.settings.showPreviewCamera()
      ]);

      this.activeRoom =
        await this.videoChatService
          .joinOrCreateRoom(roomName, tracks);

      this.participants.initialize(this.activeRoom.participants);
      this.registerRoomEvents();

      this.notificationHub.send('RoomsUpdated', true);
    }
  }

  onParticipantsChanged(_: boolean) {
    this.videoChatService.nudge();
  }

  private registerRoomEvents() {
    this.activeRoom
      .on('disconnected',
        (room: Room) => room.localParticipant.tracks.forEach(publication => this.detachLocalTrack(publication.track)))
      .on('participantConnected',
        (participant: RemoteParticipant) => this.participants.add(participant))
      .on('participantDisconnected',
        (participant: RemoteParticipant) => this.participants.remove(participant))
      .on('dominantSpeakerChanged',
        (dominantSpeaker: RemoteParticipant) => this.participants.loudest(dominantSpeaker));

  }

  private detachLocalTrack(track: LocalTrack) {
    if (this.isDetachable(track)) {
      track.detach().forEach(el => el.remove());
    }
  }

  private isDetachable(track: LocalTrack): track is LocalAudioTrack | LocalVideoTrack {
    return !!track
      && ((track as LocalAudioTrack).detach !== undefined
        || (track as LocalVideoTrack).detach !== undefined);
  }

  onLogout() {
    localStorage.removeItem('tokenAuth');
    this.router.navigate(['/user/login']);
  }

  voiceBotStart() {
    var i = 12;
    let recognition = new (<any>window).webkitSpeechRecognition();
    recognition.continuous = true;
    recognition.lang = 'en-US';
    recognition.interimResults = false;
    recognition.maxAlternatives = 1;
    this.txt = "Hello ! We are going to start the quiz ! Are you ready ?";
    var utterThis = new SpeechSynthesisUtterance(this.txt);
    const textWrapper = document.getElementById('an-1');
    textWrapper.innerHTML = this.txt;
    speechUtteranceChunker(utterThis, { chunkLength: 120 }, e => {
      recognition.start();
      this._beep.play()
      recognition.onresult = event => {
        var transcript = event.results[event.results.length - 1][0].transcript.trim();
        recognition.stop();
        console.log("You said : " + transcript);
        speakAgain(i, this.Questions, this._beep, this.synth, this.answer, this.risk, this.id_invest, this.router, this.http, this.txt, this.userDetails);
      }
    });
  }
}


function endOfQuiz(answer, risk, id_invest, router, http, userdetails) {
  var k = true;
  for (var i = 1; i < 12; i++) {
    if (answer[i].Choice != null) {
      k = true;
    }
    else {
      k = false;
    }
  }

  if (k == true) {
    console.log("answer", answer);
    http.post('/api/Investisseur', { "ID_INVEST": id_invest, "NAME": userdetails['FullName'], "LASTNAME": userdetails['UserName'], "RISK": risk, "CODE": id_invest, "Quest_Invest": null }).subscribe(result => { console.log(result) });
    http.post('/api/Questionnaire', { "Id_invest": id_invest, "version": 0, "answer": answer, "RoomSid": null }).subscribe(result => { console.log("Quiz posted !") });
    setTimeout(() => {
      http.get('/api/Risk/' + id_invest).subscribe((result) => {
        if (result >= 12 && result <= 36) {
          risk = 1;
        }

        if (result >= 37 && result <= 72) {
          risk = 3;
        }

        if (result >= 73 && result <= 108) {
          risk = 5;
        }

        if (result >= 109 && result <= 144) {
          risk = 7;
        }

        if (result > 144) {
          risk = 9;
        }
        console.log("risk", risk);
        http.put('/api/Investisseur/' + id_invest + '/' + risk).subscribe(result => { console.log(result) });
      });
    }, 3000);
  }

  if (k == false) {
    alert('Please answer all the questions');
  }
}

function speakAgain(i: number, Questions, beep, synth, answer, risk, id_invest, router, http, txt, userdetails) {
  var choices = [];
  let recognition = new (<any>window).webkitSpeechRecognition();
  recognition.continuous = true;
  recognition.lang = "en-US";
  recognition.interimResults = false;
  recognition.maxAlternatives = 1;
  let transcript = "";
  txt = "Question " + i + " : ";
  txt += Questions[i - 1].question + " .\n ";
  for (var j = 0; j < Questions[i - 1].choiceList.length; j++) {
    var choice = Questions[i - 1].choiceList[j].choice;
    choices.push(choice);
    choice = choice.replaceAll(',0', '00');
    choice = choice.replaceAll(',5', '50');
    choice = choice.replaceAll('- ', '-');
    txt += choice + " .\n ";
  }
  var utterThis = new SpeechSynthesisUtterance(txt);
  const textWrapper = document.getElementById('an-1');
  textWrapper.innerHTML = txt.replace(/(?:\r\n|\r|\n)/g, '<br>');
  speechUtteranceChunker(utterThis, { chunkLength: 120 }, e => {
    recognition.start();
    recognition.onstart = event => {
      beep.play()
    }
    recognition.onresult = event => {
      transcript = event.results[event.results.length - 1][0].transcript.trim();
      var confidence = event.results[event.results.length - 1][0].confidence;
      recognition.stop();
      console.log("You said : " + transcript + " with confidence = " + confidence);
      var res = transcript.split(" ");
      var index = 0;
      var max = 0;
      for (let a = 0; a < choices.length; a++) {
        var aux = 0;
        for (let j = 0; j < res.length; j++) {
          if ((choices[a].toLowerCase()).includes(res[j])) {
            aux++;
          }
        }
        if (max <= aux) {
          max = aux;
          index = a;
        }
      }
      console.log("Votre choix est : " + choices[index]);
      answer[(i - 1)].Choice = choices[index];
      console.log('afterpush', answer);
      if (i < Questions.length) {
        i++;
        speakAgain(i, Questions, beep, synth, answer, risk, id_invest, router, http, txt, userdetails);
      }
      else {
        textWrapper.innerHTML = "End of Quiz ! Thank you !";

        var utterThis = new SpeechSynthesisUtterance("End of Quiz ! Thank you !");
        speechUtteranceChunker(utterThis, { chunkLength: 120 }, e => {
          endOfQuiz(answer, risk, id_invest, router, http, userdetails);
        });
      }
    }
  });
}
