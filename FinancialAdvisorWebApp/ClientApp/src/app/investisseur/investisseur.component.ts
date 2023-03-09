import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import Chart from 'chart.js/auto';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-investisseur',
  templateUrl: './investisseur.component.html',
  styleUrls: ['./investisseur.component.css']
})
export class InvestisseurComponent implements OnInit {
  private notificationHub: HubConnection;
  public id_invest;
  public userDetails;
  public Emotions: emotion;
  Investor;
  public maxEmotion;
  public Questionnaire;
  public location = "";
  constructor(private http: HttpClient, private service: UserService) { }

  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
        this.id_invest = this.userDetails['id'];

        this.http.get('/api/Investisseur/' + this.id_invest).subscribe((investor) => {
          this.Investor = investor;

          this.http.get('/api/video/speechemotions/' + this.id_invest).subscribe((speech) => {
            if (speech != null) {
              this.maxEmotion = speech['emotion'];

              this.http.get<emotion>('/api/video/emotions/' + this.id_invest).subscribe((result) => {
                this.Emotions = null;
                if (result != null) {
                  this.http.get('/api/Questionnaire/version/' + this.id_invest).subscribe((version) => {
                    console.log("**************  " + result.version);
                    if (version != null) {
                      if (result.version != version['version'])
                        this.Emotions = null;
                      else
                        this.Emotions = result;
                    }
                    else
                      console.log("There is no investor with this name !");
                  }, error => console.error(error));
                }
                else
                  console.log("There is no investor with this name !");
              }, error => console.error(error));
            }
            else
              console.log("There is no investor with this name !");
          }, error => console.error(error));
        }, error => console.error(error));

        this.http.get('/api/Questionnaire/' + this.id_invest).subscribe((quest) => {
          if (quest != null)
            this.Questionnaire = quest;
          else
            console.log("There is no investor with this name !");
        }, error => console.error(error));

      },
      err => {
        console.log(err);
      },
    );
    //if (this.Emotions != null) {
      setTimeout(() => {
        var myChart = new Chart("myChart", {
          type: 'bar',
          data: {
            labels: ['Angry', 'Disgust', 'Scared', 'Happy', 'Sad', 'Surprised', 'Neutral'],
            datasets: [{
              data: [this.Emotions.angry, this.Emotions.disgust, this.Emotions.scared, this.Emotions.happy, this.Emotions.sad, this.Emotions.surprised, this.Emotions.neutral],
              backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)',
                'rgba(128, 0, 128, 0.2)'
              ],
              borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)',
                'rgba(128, 0, 128, 1)'

              ],
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            },
            plugins: {
              title: {
                display: true,
                text: 'Average of facial emotions recognition'
              },
              legend: {
                display: false
              }
            }
          }
        });
      }, 2000);
    //}
  }

}

interface emotion {
  iD_FACE_EMOTION: number;
  iD_INVEST: string;
  version: string;
  angry: number;
  disgust: number;
  scared: number;
  happy: number;
  sad: number;
  surprised: number;
  neutral: number;
  deviation: string;
}
