import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private accounService: AccountService) {}

  ngOnInit(): void {}

  register() {
    this.accounService.register(this.model).subscribe({
      next: (res) => {
        console.log(res);
        this.cancel;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
