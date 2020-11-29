import { Component, OnInit } from '@angular/core';
import { FormArray, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/_model/Evento';
import { EventoService } from 'src/app/_services/evento.service';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.scss']
})
export class EventoEditComponent implements OnInit {
  titulo = 'Editar Evento';
  registerForm: FormGroup;
  evento: Evento = new Evento();
  imagemURL = 'assets/img/upload.png';
  file: File;
  fileNameToUpdate: string;
  dataAtual = '';

  get lotes(): FormArray{
    // tslint:disable-next-line:no-angle-bracket-type-assertion
    return <FormArray> this.registerForm.get('lotes');
  }

  get redesSociais(): FormArray{
    // tslint:disable-next-line:no-angle-bracket-type-assertion
    return <FormArray> this.registerForm.get('redesSociais');
  }
  constructor(
    private eventoService: EventoService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService,
    private route: ActivatedRoute
    ) {
      this.localeService.use('pt-br');
    }

  ngOnInit(): void{
    this.validation();
    this.carregarEvento();
    }


    carregarEvento(): void{
      const idEvento = +this.route.snapshot.paramMap.get('id');
      this.eventoService.getEventoById(idEvento)
      .subscribe(
         (evento: any) => {
          this.evento = Object.assign({}, evento);
          this.fileNameToUpdate = evento.imagemURL.toString();
          this.imagemURL = `http://localhost:5000/resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;

          this.evento.imagemURL = '';
          this.registerForm.patchValue(this.evento);

          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criaLote(lote));
          });

          this.evento.redesSociais.forEach(redeSocial => {
            this.redesSociais.push(this.criaRedesSocial(redeSocial));
          });
        }

      );
    }

    validation(): void{
      this.registerForm = this.fb.group({
        id: [],
        tema:  ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
        imagemURL: [''],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        lotes: this.fb.array([]) ,
        redesSociais: this.fb.array([])
      });
    }
   criaLote(lote: any): FormGroup{
     return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
      });
   }

   criaRedesSocial(redeSocial: any): FormGroup{
     return  this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial. url, Validators.required]
    });
   }


   adicionarLote(): void{
     this.lotes.push(this.criaLote({ id: 0 }));
   }

   adicionarRedeSocial(): void{
     this.redesSociais.push(this.criaRedesSocial({ id: 0 }));
   }

   removerLote(id: number): void{
     this.lotes.removeAt(id);
   }

   removerRedeSocial(id: number): void {
     this.redesSociais.removeAt(id);
   }

    onFileChange(evento: any, file: FileList): void {
     const reader = new FileReader();
     reader.onload = (event: any) => this.imagemURL = event.target.result;
     this.file = evento.target.files;
     reader.readAsDataURL(file[0]);
    }

    salvarEvento(): void{

      this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);
      this.evento.imagemURL = this.fileNameToUpdate;

      this.uploadImagem();

      this.eventoService.putEvento(this.evento).subscribe(
      () => {
        this.toastr.success('Editado com Sucesso!');
      }, error => {
        this.toastr.error(`Erro ao Editar: ${error}`);
      }
    );

    }

    uploadImagem(): void{

      if (this.registerForm.get('imagemURL').value !== '') {
        this.eventoService.postUpload(this.file, this.fileNameToUpdate)
          .subscribe(
            () => {
              this.dataAtual = new Date().getMilliseconds().toString();
              this.imagemURL = `http://localhost:5000/resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;
            }
          );
      }

    }

}
