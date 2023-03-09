import { Component, OnInit } from '@angular/core';
import { UserService } from './../shared/user.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

declare const speechUtteranceChunker: any;

@Component({
    selector: 'app-home',
    styleUrls: ['./home.component.css'],
    templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

    public userDetails = null;
    _beep = new window.Audio("assets/beep.wav");
    synth = window.speechSynthesis;
    Questions;
    public id_invest;
    public risk: number;
    dateDay = new Date();
    constructor(private router: Router, private service: UserService, private http: HttpClient) { }

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

    onLogout() {
        localStorage.removeItem('tokenAuth');
        this.router.navigate(['/user/login']);
    }

    makeCall() {
        this.router.navigate(['/consultation']);
    }
}