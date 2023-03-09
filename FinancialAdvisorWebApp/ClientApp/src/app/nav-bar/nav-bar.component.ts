import { Component, OnInit } from '@angular/core';
import { UserService } from './../shared/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  userDetails;
  dateDay = new Date().getDate()+" - "+new Date().getMonth()+" - "+new Date().getFullYear();


  constructor(private router: Router, private service: UserService) { }

  async ngOnInit() {
    this.service.getUserProfile().subscribe(
        res => {
            this.userDetails = res;
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

}
