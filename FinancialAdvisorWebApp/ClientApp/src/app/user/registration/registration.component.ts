import { UserService } from './../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onBlurMethod(event) {
    const inputValue = event.target.value;
    if (inputValue != "") {
      event.target.classList.add('has-val');
    }
    else {
      event.target.classList.remove('has-val');
    }
  }

  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        localStorage.setItem('tokenAuth', res.token);
        this.router.navigateByUrl('/home');
      },
      err => {
        console.log(err);
      }
    );
  }

}
