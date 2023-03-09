import { ToastrService } from 'ngx-toastr';
import { UserService } from './../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  }
  constructor(private service: UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    if (localStorage.getItem('tokenAuth') != null)
      this.router.navigateByUrl('/home');
    
    this.verifyInput((<HTMLInputElement>document.getElementById("Password")));
    this.verifyInput((<HTMLInputElement>document.getElementById("UserName")));
  }

  onBlurMethod(event) {
    const inputValue = event.target.value;
    if (inputValue != "") {
      event.target.classList.add('has-val');
    }
    else
    {
      event.target.classList.remove('has-val');
    }
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('tokenAuth', res.token);
        this.router.navigateByUrl('/home');
      },
      err => {
        if (err.status == 400)
          this.toastr.error('Incorrect username or password.', 'Authentication failed.');
        else
          console.log(err);
      }
    );
  }

  verifyInput(input)
  {
    if (input.value != "") {
      input.classList.add('has-val');
    }
  }
}
