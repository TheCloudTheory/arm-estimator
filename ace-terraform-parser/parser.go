package main

import "C"

import (
	"context"
	"encoding/json"
	"log"
	"os"
	"os/exec"

	"github.com/hashicorp/terraform-exec/tfexec"
)

//export GenerateParsedPlan
func GenerateParsedPlan(workingDir string, planFile string) {
	path, err := exec.LookPath("terraform")
	if err != nil {
		log.Printf("error looking for Terraform executable: %s", err)
		return
	}

	tf, err := tfexec.NewTerraform(workingDir, path)
	if err != nil {
		log.Printf("error running NewTerraform: %s", err)
		return
	}

	plan, err := tf.ShowPlanFile(context.Background(), planFile)
	if err != nil {
		log.Printf("error running Show: %s", err)
		return
	}

	ret, err := json.Marshal(plan)
	if err != nil {
		log.Printf("error marshalling json: %s", err)
		return
	}

	os.WriteFile("ace-terraform-parsed-plan.json", ret, 0644)
}

func main() {
}
