import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_model/User';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup;
  user: User;

  constructor(private authService: AuthService
            , public router: Router
            , public fb: FormBuilder
            , private toastr: ToastrService) { }

  // tslint:disable-next-line:typedef
  ngOnInit() {
    this.validation();
  }

  // tslint:disable-next-line:typedef
  validation(){
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      passwords: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', Validators.required]
      }, { validator : this.compararSenhas})
    });
  }

  // tslint:disable-next-line:typedef
  compararSenhas(fb: FormGroup){
    const confirmSenhaCtrl = fb.get('confirmPassword');
    if (confirmSenhaCtrl.errors === null || 'mismatch' in confirmSenhaCtrl.errors)
    {
      if (fb.get('password').value !== confirmSenhaCtrl.value){
        confirmSenhaCtrl.setErrors({mismatch: true});
      }else{
        confirmSenhaCtrl.setErrors(null);
      }
    }

  }
  // tslint:disable-next-line:typedef
  cadastrarUsuario(){
    if (this.registerForm.valid){
        this.user = Object.assign({password: this.registerForm.get('passwords.password').value},
        this.registerForm.value);
        this.authService.register(this.user).subscribe(

          () => {
            this.router.navigate(['/user/login']);
            this.toastr.success('Cadastrado com Succeso!');
          },
          error => {
            const erro = error.error;
            erro.forEach(element => {
              switch (element.code) {
                case 'DuplicateUserName':
                  this.toastr.error('Cadastro Duplicado!');
                  break;

                default:
                  this.toastr.error(`Error no Cadastro! CODE: ${element.code}`);
                  break;
              }
            });

          }
        );
    }
  }

}
